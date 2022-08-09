using System;
using System.Collections.Generic;
using Game.Battle;
using Game.Battle.Map;
using Game.DataSet;
using Game.Unit.Ability;
using Game.Unit.Part;
using Game.Unit.Skill;
using Game.Unit.StatusEffect;
using TMPro;
using UnityEngine;


namespace Game.Unit
{
    public class UnitObject : MonoBehaviour
    {
        [SerializeField] private SkillDataSetSO _skillDataSet;
        [SerializeField] private BattleService _battleService;

        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private List<StatusEffectRegister> _statusEffectRegisters;

        private UnitSO unitSO;
        
        public UnitParam param;
        public UnitAnimation anim { get; private set; }
        public UnitPartTree partTree { get; private set; }
        public SpriteRenderer spriteRenderer { get => _spriteRenderer; }
        public Transform _transform { get; private set; }
        public BattleTeam _team { get => BattleTeam.Player; }

        public event Action<AttackInfo> OnInitiatingAttack;
        public event Action<AttackInfo> OnRecivingAttack;
        public event Action<AttackInfo> OnStartTakenAttack;
        public event Action<DamageInfo> OnStartTakenDamage;
        public event Action<AttackInfo> OnTakenAttack;
        public event Action<DamageInfo> OnTakenDamage;
        public event Action<AttackInfo> OnStartDealDamage;
        public event Action<AttackInfo> OnDealDamage;
        public event Action<AttackInfo> OnDodgedAttack;
        public event Action<AttackInfo> OnMissedAttack;
        public event Action<AttackInfo> OnCounterAttack;
        public event Action<AttackInfo> OnExtendedAttack;
        public event Action<UnitObject> OnTurnChanged;
        public event Action<UnitObject> OnKokuChanged;

        public Vector2Int location => Vector2Int.FloorToInt(Extensions.GameV3ToV2(_transform.position));
        public float height => _transform.position.y;
        
        public void InitializeWith(UnitSO unitSO, BattleService battleService)
        {
            this.unitSO = unitSO;
            param = new UnitParam().Initialize(this);
            partTree = new UnitPartTree(this, unitSO.PartTree);
            anim = GetComponent<UnitAnimation>();
            anim.Initialize(this, unitSO.animatorOverrideController);
            _statusEffectRegisters = new List<StatusEffectRegister>();
            
            _transform = transform;
            _spriteRenderer.sprite = unitSO.sprite;
            RegisterParts(partTree.root);
            param.InitializeMaxValues();
            param.Evaluate();
            
            anim.SwitchStateTo(UnitAnimation.Idle);
            
            battleService.battleTurnManager.OnTurnChanged += InvokeOnTurnChanged;
            OnTurnChanged += RefreshKoku;
        }

        private void RefreshKoku(UnitObject _)
        {
            param.ResetMP();
        }

        private void InvokeOnTurnChanged(int turn) => OnTurnChanged?.Invoke(this);
        private void InvokeOnKokuChanged(int koku) => OnKokuChanged?.Invoke(this);

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

        public void RegisterStatusEffects(StatusEffect.StatusEffect statusEffect, object source) => RegisterStatusEffects(statusEffect, -1, source);
        public void RegisterStatusEffects(StatusEffect.StatusEffect statusEffect, int turns, object source)
        {
            _statusEffectRegisters.Add(StatusEffectRegister.From(this, statusEffect, turns, source));
        }

        public void RemoveStatusEffectRegister(StatusEffectRegister statusEffectRegister)
        {
            if (!_statusEffectRegisters.Contains(statusEffectRegister)) return;
            _statusEffectRegisters.Remove(statusEffectRegister);
        }
        
        public void RemoveStatusEffectRegister(object source)
        {
            int index = _statusEffectRegisters.FindIndex(reg => reg.source.Equals(source));
            if (index < 0) return;
            _statusEffectRegisters.RemoveAt(index);
        }
        
        public void DealDamageTo(AttackInfo attackInfo)
        {
            RollHit(attackInfo);
            
            if (attackInfo.missed)
            {
                OnMissedAttack?.Invoke(attackInfo);
                return;
            }
            
            RollCritical(attackInfo);
            
            OnStartDealDamage?.Invoke(attackInfo);
            attackInfo.target.TakeAttack(attackInfo);
            OnDealDamage?.Invoke(attackInfo);
        }
        
        public void TakeAttack(AttackInfo attackInfo)
        {
            OnRecivingAttack?.Invoke(attackInfo);
            RollDodge(attackInfo);

            if (attackInfo.dodge)
            {
                OnDodgedAttack?.Invoke(attackInfo);
                return;
            }
            
            OnStartTakenAttack?.Invoke(attackInfo);
            TakeDamage(attackInfo.damageInfo);
            OnTakenAttack?.Invoke(attackInfo);
        }
        
        public void TakeDamage(DamageInfo damageInfo)
        {
            OnStartTakenDamage?.Invoke(damageInfo);
            UnitStatModifier damageModifier = damageInfo.damageModifier;
            param.AddModifier(damageModifier);
            _battleService.uiManager.CreateDamageIndicator(_transform.position + Vector3.up, damageInfo.damageStat.Value);
            param.Evaluate();
            OnTakenDamage?.Invoke(damageInfo);
        }

        public void RollHit(AttackInfo attackInfo)
        {
            if (!param.CheckHit())
                attackInfo.ToMissed();
        }
        
        public void RollCritical(AttackInfo attackInfo)
        {
            if (param.CheckCritical())
            {
                attackInfo.ToCritical();
                
            }

        }
        
        public void RollDodge(AttackInfo attackInfo)
        {
            if (param.CheckDodge())
                attackInfo.ToDodged();
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

    [Serializable]
    public struct StatusEffectRegister
    {
        public StatusEffect.StatusEffect statusEffect { get; private set; }
        public int turnsLeft { get; private set; }
        public UnitObject Unit => statusEffect.unit;
        public object source { get; private set; }

        public void CountDown(UnitObject _)
        {
            if (turnsLeft <= -1) return;
            
            turnsLeft--;
            if (turnsLeft == 0)
            {
                Unit.RemoveStatusEffectRegister(this);
                statusEffect.Remove();
                Unit.OnTurnChanged -= CountDown;
            }
        }
        
        public static StatusEffectRegister From(UnitObject _unit, StatusEffect.StatusEffect statusEffect, int turns, object _source)
        {
            StatusEffectRegister reg = new StatusEffectRegister()
            {
                turnsLeft = turns,
                source = _source,
                statusEffect = statusEffect,
            };
            
            statusEffect.RegisterTo(_unit);
            reg.Unit.OnTurnChanged += reg.CountDown;
            return reg;
        }
    }
    
    public struct DamageInfo
    {
        public DamageStat damageStat { get; private set; }
        public object source { get; private set; }
        
        public void AddModifier(DamageStatModifier damageStatModifier) => damageStat.AddModifier(damageStatModifier);
        public UnitStatModifier damageModifier => 
            new UnitStatModifier(UnitStatType.DUR, -damageStat.Value, BaseStatModifier.ModifyType.Flat, source);
        
        public static DamageInfo From(object _source) => new DamageInfo()
        {
            damageStat = new DamageStat(),
            source = _source,
        };
    }
    
    public class AttackInfo
    {
        public AttackSourceInfo source { get; private set; }
        public BattleBoardTile targetTile { get; private set; }
        public UnitObject target { get => targetTile.unitOnTile; }
        public DamageInfo damageInfo { get; private set; }
        public int combo { get; private set; }
        public bool isCritical { get; private set; }
        public bool missed { get; private set; }
        public bool dodge { get; private set; }

        public static AttackInfo From(AttackSourceInfo _source, BattleBoardTile _targetTile) =>
            From(_source, _targetTile, 1);

        public static AttackInfo From(AttackSourceInfo _source, BattleBoardTile _targetTile, int _combo) => new AttackInfo()
        {
            source = _source,
            targetTile = _targetTile,
            damageInfo = DamageInfo.From(_source),
            combo = _combo,
            isCritical = false,
            missed = false,
            dodge = false,
        };

        public bool ToCritical() => isCritical = true;
        public bool ToMissed() => missed = true;
        public bool ToDodged() => dodge = true;
        
        public void AddModifier(DamageStatModifier damageStatModifier) => damageInfo.AddModifier(damageStatModifier);

        public UnitStatModifier damageModifier => damageInfo.damageModifier;
    }

    public struct AttackSourceInfo
    {
        public BattleBoardTile sourceTile { get; private set; }
        public UnitObject sourceUnit { get; private set; }
        public SkillSO sourceSkill { get; private set; }

        public static AttackSourceInfo Empty => new AttackSourceInfo()
        {
            sourceTile = null,
            sourceUnit = null,
            sourceSkill = null,
        };
        
        public static AttackSourceInfo From(SkillCastInfo skillCastInfo) =>
            From(skillCastInfo.casterTile, skillCastInfo.castedSkill);
        
        public static AttackSourceInfo From(BattleBoardTile sourceTile, SkillSO _sourceSkill) => new AttackSourceInfo()
        {
            sourceTile = sourceTile,
            sourceUnit = sourceTile.unitOnTile,
            sourceSkill = _sourceSkill,
        };
    }
}