using System;
using System.Collections.Generic;
using Game.Battle;
using Game.Battle.Map;
using Game.DataSet;
using Game.Unit.Ability;
using Game.Unit.Part;
using Game.Unit.Skill;
using UnityEngine;


namespace Game.Unit
{
    public class UnitObject : MonoBehaviour
    {
        [SerializeField] private SkillDataSetSO _skillDataSet;
        [SerializeField] private BattleService _battleService;

        [SerializeField] private SpriteRenderer _spriteRenderer;

        public UnitSO unitSO { get; private set; }
        
        // to see in editor
        public UnitParam param;
        public UnitAnimation anim { get; private set; }
        public UnitPartTree partTree { get; private set; }
        public SpriteRenderer spriteRenderer { get => _spriteRenderer; }
        public Transform _transform { get; private set; }
        public CpuUnitController cpuUnitController { get; private set; }
        public UnitSEHandler seHandler { get; private set; }
        public BattleTeam _team { get => BattleTeam.Player; }
        
        public string displayName { get; private set; }

        public int gridX => Mathf.FloorToInt(_transform.position.x);
        public int gridY => Mathf.FloorToInt(_transform.position.z);

        public Vector2Int location => Vector2Int.FloorToInt(Extensions.GameV3ToV2(_transform.position));
        public float height => _transform.position.y;

        #region UnitGameEvents
        public event Action<AttackInfo> OnAttackEarly = delegate { };
        public event Action<AttackInfo> OnAttack = delegate { };
        public event Action<AttackInfo> OnAttackLate = delegate { };
        public event Action<AttackInfo> OnTakeAttackEarly = delegate { };
        public event Action<AttackInfo> OnTakeAttack = delegate { };
        public event Action<AttackInfo> OnTakeAttackLate = delegate { };
        public event Action<DamageInfo> OnTakeDamageEarly = delegate { };
        public event Action<DamageInfo> OnTakeDamageLate = delegate { };
        public event Action<UnitObject> OnTurnChanged = delegate { };
        public event Action<UnitObject> OnKokuChanged = delegate { };
        public event Action<UnitObject> OnActionEnd = delegate { };
        public event Action<AttackInfo> OnMissedAttack = delegate { };
        public event Action<AttackInfo> OnDodgedAttack = delegate { };
        #endregion
        
        #region UnitAbilityEvents
        public event Action<AttackInfo> OnAbAttackEarly = delegate { };
        public event Action<AttackInfo> OnAbAttack = delegate { };
        public event Action<AttackInfo> OnAbAttackLate = delegate { };
        public event Action<AttackInfo> OnAbTakeAttackEarly = delegate { };
        public event Action<AttackInfo> OnAbTakeAttack = delegate { };
        public event Action<AttackInfo> OnAbTakeAttackLate = delegate { };
        public event Action<DamageInfo> OnAbTakeDamageEarly = delegate { };
        public event Action<DamageInfo> OnAbTakeDamageLate = delegate { };
        public event Action<UnitObject> OnAbTurnChanged = delegate { };
        public event Action<UnitObject> OnAbKokuChanged = delegate { };
        public event Action<UnitObject> OnAbActionEnd = delegate { };
        public event Action<AttackInfo> OnAbMissedAttack = delegate { };
        public event Action<AttackInfo> OnAbDodgedAttack = delegate { };
        #endregion
        
        #region UnitSEEvents
        public event Action<AttackInfo> OnSEAttackEarly = delegate { };
        public event Action<AttackInfo> OnSEAttack = delegate { };
        public event Action<AttackInfo> OnSEAttackLate = delegate { };
        public event Action<AttackInfo> OnSETakeAttackEarly = delegate { };
        public event Action<AttackInfo> OnSETakeAttack = delegate { };
        public event Action<AttackInfo> OnSETakeAttackLate = delegate { };
        public event Action<DamageInfo> OnSETakeDamageEarly = delegate { };
        public event Action<DamageInfo> OnSETakeDamageLate = delegate { };
        public event Action<UnitObject> OnSETurnChanged = delegate { };
        public event Action<UnitObject> OnSEKokuChanged = delegate { };
        public event Action<UnitObject> OnSEActionEnd = delegate { };
        public event Action<AttackInfo> OnSEMissedAttack = delegate { };
        public event Action<AttackInfo> OnSEDodgedAttack = delegate { };
        #endregion
        
        public void InitializeWith(UnitSO _unitSO, BattleService battleService)
        {
            // setup SO values
            unitSO = _unitSO;
            displayName = unitSO.displayName;
            _spriteRenderer.sprite = unitSO.sprite;
            
            // setup unit values
            _transform = transform;

            // setup parts and abilities
            param = new UnitParam().Initialize(this);
            partTree = new UnitPartTree(this, unitSO.PartTree);
            
            // set cpu ai
            cpuUnitController = GetComponent<CpuUnitController>();
            cpuUnitController.SetAI(unitSO.ai);
            
            // setup animation
            anim = GetComponent<UnitAnimation>();
            anim.Initialize(this, unitSO.animatorOverrideController);
            anim.SwitchStateTo(UnitAnimation.Idle);
            
            // set up SEs
            seHandler = new UnitSEHandler(this);

            RegisterParts(partTree.root);
            
            param.InitializeMaxValues();
            param.Evaluate();
            
            // events
            battleService.battleTurnManager.OnTurnChanged += InvokeOnTurnChanged;
        }
        
        private void InvokeOnTurnChanged(int turn)
        {
            param.ResetAP();
            OnTurnChanged.Invoke(this);
            OnAbTurnChanged.Invoke(this);
            OnSETurnChanged.Invoke(this);
        }

        private void InvokeOnKokuChanged(int koku)
        {
            OnKokuChanged.Invoke(this);
            OnAbKokuChanged.Invoke(this);
            OnSEKokuChanged.Invoke(this);
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

        public void EndAction()
        {
            OnActionEnd.Invoke(this);
        }
        
        public void Attack(AttackInfo attackInfo)
        {
            OnAbAttackEarly.Invoke(attackInfo);
        
            RollHit(attackInfo);
            
            if (attackInfo.missed)
            {
                OnMissedAttack.Invoke(attackInfo);
                OnAbMissedAttack.Invoke(attackInfo);
                return;
            }
            
            RollCritical(attackInfo);
            
            OnAbAttack.Invoke(attackInfo);
            attackInfo.target.TakeAttack(attackInfo);
            
            _battleService.logConsole.SendText($"{name} deal {attackInfo.damageModifier.value} damage to {attackInfo.target.name}");
            OnAbAttackLate.Invoke(attackInfo);
        }
        
        public void TakeAttack(AttackInfo attackInfo)
        {
            OnAbTakeAttackEarly.Invoke(attackInfo);
            RollDodge(attackInfo);

            if (attackInfo.dodge)
            {
                OnDodgedAttack.Invoke(attackInfo);
                OnAbDodgedAttack.Invoke(attackInfo);
                return;
            }
            
            OnAbTakeAttack.Invoke(attackInfo);
            TakeDamage(attackInfo.damageInfo);
            OnAbTakeAttackLate.Invoke(attackInfo);
        }
        
        public void TakeDamage(DamageInfo damageInfo)
        {
            OnAbTakeDamageEarly.Invoke(damageInfo);
            UnitStatModifier damageModifier = damageInfo.damageModifier;
            param.AddModifier(damageModifier);
            _battleService.BattleUIManager.CreateDamageIndicator(_transform.position + Vector3.up, damageInfo.damageStat.Value);
            param.Evaluate();
            OnAbTakeDamageLate.Invoke(damageInfo);
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