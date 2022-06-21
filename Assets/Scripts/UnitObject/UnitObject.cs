using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Battle;
using Game.Battle.Map;
using Game.DataSet;
using Game.Unit.Ability;
using Game.Unit.Part;
using Game.Unit.Skill;
using Game.Unit.StatusEffects;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Unit
{
    public class UnitObject : MonoBehaviour
    {
        [SerializeField] private SkillDataSetSO _skillDataSet;
        [SerializeField] private StatusEffectDataSetSO _statusEffectDataSet;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        private UnitSO unitSO;
        
        public string displayName { get; private set; }
        public Sprite sprite { get; private set; }
        public UnitId unitId { get; private set; }
        public UnitParam param;
        public UnitAnimation unitAnimation { get; private set; }
        public UnitPartTree partTree { get; private set; }

        public event Action<DamageInfo> OnStartTakenDamage;
        public event Action<DamageInfo> OnTakenDamage;
        public event Action<DamageInfo> OnStartDealDamage;
        public event Action<DamageInfo> OnDealDamage;

        public Vector2Int location => Vector2Int.FloorToInt(Extensions.GameV3ToV2(transform.position));
        public float height => transform.position.y;
        
        public void InitializeWith(UnitSO unitSO)
        {
            this.unitSO = unitSO;
            param = new UnitParam().Initialize(this);
            partTree = new UnitPartTree(this, unitSO.PartTree);
            unitAnimation = GetComponent<UnitAnimation>();

            _spriteRenderer.sprite = unitSO.sprite;
            RegisterParts(partTree.root);
            param.InitializeMaxValues();
            param.Evaluate();
        }

        private void RegisterParts(UnitPartTree.UnitPartTreeNode node)
        {
            RegisterPart(node);
            if (node.Children == null) return;
            for (int i = 0; i < node.Children.Length; i++)
                RegisterParts(node.Children[i]);
        }
        
        private void RegisterPart(UnitPartTree.UnitPartTreeNode node)
        {
            if (node.Part == null)
            {
                Debug.LogWarning($"{"null part data!!"}");
                return;
            }
            
            AbilitySO[] abilities = node.Part.GetAbilities();
            for (int i = 0; i < abilities.Length; i++)
                abilities[i].RegisterTo(this, node);
            param.AddModifiers(node.Part.statBoost);
        }

        public void RegisterStatusEffects(StatusEffectId statusEffectId)
        {
            _statusEffectDataSet[statusEffectId].RegisterTo(this);
        }
        
        public void RemoveStatusEffects(StatusEffectId statusEffectId)
        {
            _statusEffectDataSet[statusEffectId].RemoveFrom(this);
        }
        
        public void DealDamageTo(DamageInfo damageInfo)
        {
            OnStartDealDamage?.Invoke(damageInfo);
            damageInfo.target.TakeDamage(damageInfo);
            OnDealDamage?.Invoke(damageInfo);
        }
        
        public void TakeDamage(DamageInfo damageInfo)
        {
            OnStartTakenDamage?.Invoke(damageInfo);
            param.AddModifier(damageInfo.damageModifier);
            param.Evaluate();
            OnTakenDamage?.Invoke(damageInfo);
        }
        
        public class UnitPartTree
        {
            public class UnitPartTreeNode
            {
                public PartSO Part { get; }
                public UnitPartTreeNode[] Children { get; }
    
                public UnitPartTreeNode(List<UnitPartTreeNode> nodes, PartNode partNode)
                {
                    nodes.Add(this);
                    Part = partNode.part;
                    if (partNode.children.Enabled)
                    {
                        Children = new UnitPartTreeNode[partNode.children.Value.Length];
                        for (int i = 0; i < partNode.children.Value.Length; i++)
                        {
                            Children[i] = new UnitPartTreeNode(nodes, partNode.children.Value[i]);
                        }
                    }
                }
            }

            public UnitObject unit;
            public UnitPartTreeNode root;
            public List<UnitPartTreeNode> nodes;
            
            public UnitPartTree(UnitObject _unit, PartNode partTree)
            {
                unit = _unit;
                nodes = new List<UnitPartTreeNode>();
                root = new UnitPartTreeNode(nodes, partTree);
            }
    
            public List<SkillId> GetAllSkillIds()
            {
                List<SkillSO> skills = GetAllSkills();
                List<SkillId> skillIds = new List<SkillId>();
                foreach (SkillSO skill in skills)
                {
                    skillIds.Add(skill.id);
                }
            
                return skillIds;
            }
            
            public List<SkillSO> GetAllSkills()
            {
                List<SkillSO> skills = new List<SkillSO>();
                foreach (UnitPartTreeNode node in nodes)
                {
                    if (node.Part.skillId == SkillId.None)
                        continue;
                    
                    SkillSO skill = unit._skillDataSet[node.Part.skillId];
                    if (!(skill.unique && skills.Contains(skill)))
                        skills.Add(skill);
                }
    
                return skills;
            }
        }
    }
    
    public struct DamageInfo
    {
        public DamageSourceInfo source { get; private set; }
        public BattleBoardTile targetTile { get; private set; }
        public UnitObject target { get => targetTile.unitOnTile; }
        public DamageStat damageStat;

        public static DamageInfo From(DamageSourceInfo _source, BattleBoardTile _targetTile) => new DamageInfo()
        {
            source = _source,
            targetTile = _targetTile,
            damageStat = new DamageStat(),
        };

        public void AddModifier(DamageStatModifier damageStatModifier) => damageStat.AddModifier(damageStatModifier);

        public UnitStatModifier damageModifier =>
            new UnitStatModifier(UnitStatType.DUR, -damageStat.Value, BaseStatModifier.ModifyType.Flat, source);
    }

    public struct DamageSourceInfo
    {
        public BattleBoardTile sourceTile { get; private set; }
        public UnitObject sourceUnit { get; private set; }
        public SkillSO sourceSkill { get; private set; }

        public static DamageSourceInfo From(SkillCastInfo skillCastInfo) =>
            From(skillCastInfo.casterTile, skillCastInfo.castedSkill);
        public static DamageSourceInfo From(BattleBoardTile sourceTile, SkillSO _sourceSkill) => new DamageSourceInfo()
        {
            sourceTile = sourceTile,
            sourceUnit = sourceTile.unitOnTile,
            sourceSkill = _sourceSkill,
        };
    }
}