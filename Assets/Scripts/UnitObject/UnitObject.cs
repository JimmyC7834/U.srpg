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
using UnityEngine;
using UnityEngine.Events;

namespace Game.Unit
{
    public class UnitObject : MonoBehaviour
    {
        [SerializeField] private SkillDataSetSO _skillDataSet;
            
        private UnitSO unitSO;
        
        public string displayName { get; private set; }
        public Sprite sprite { get; private set; }
        public UnitId unitId { get; private set; }
        public UnitParam unitParam;
        public UnitAnimation unitAnimation { get; private set; }
        public UnitPartTree partTree { get; private set; }

        public event Action<DamageInfo> OnTakenDamage;
        public event Action<DamageInfo> OnDealDamage;

        public Vector2Int location => Vector2Int.FloorToInt(Extensions.GameV3ToV2(transform.position));
        
        public void InitializeWith(UnitSO unitSO)
        {
            this.unitSO = unitSO;
            unitParam = new UnitParam().Initialize(unitSO);
            unitAnimation = GetComponent<UnitAnimation>();
            partTree = new UnitPartTree(this, unitSO.PartTree);
            RegisterParts(partTree.root);
            unitParam.Evaluate();
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
            unitParam.AddModifiers(node.Part.statBoost);
        }
        
        public void DealDamage(DamageInfo damageInfo)
        {
            unitParam.AddModifiers(damageInfo.damgeModifiers);
            unitParam.Evaluate();
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

            public UnitObject unitObject;
            public UnitPartTreeNode root;
            public List<UnitPartTreeNode> nodes;
            
            public UnitPartTree(UnitObject _unitObject, PartNode partTree)
            {
                unitObject = _unitObject;
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
                    
                    SkillSO skill = unitObject._skillDataSet[node.Part.skillId];
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
        public BattleBoardTile toTile { get; private set; }
        public List<UnitStatModifier> damgeModifiers { get; private set; }

        public static DamageInfo From(DamageSourceInfo _source, BattleBoardTile _toTile) => new DamageInfo()
        {
            source = _source,
            toTile = _toTile,
            damgeModifiers = new List<UnitStatModifier>(),
        };

        public void AddModifier(UnitStatModifier unitStatModifier) => damgeModifiers.Add(unitStatModifier);
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