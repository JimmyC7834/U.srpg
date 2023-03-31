using System.Linq;
using Game.Battle;
using Game.DataSet;
using Game.Unit.Skill;
using Game.Unit.StatusEffect;
using JetBrains.Annotations;
using UnityEngine;

namespace Game.Unit
{
    /**
     * GameObject of a unit
     */
    public class UnitObject : MonoBehaviour
    {
        [SerializeField] private LogConsole _logConsole;
        [SerializeField] private SkillDataSetSO _skillDataSet;
        private Transform _transform;
        
        public UnitSO unitSO { get; private set; }
        public CpuUnitController cpuUnitController { get; private set; }
        public UnitAnimation Anim { get; private set; }
        
        public UnitStats Stats { get => Data.Stats; }
        public BattleTeam Team { get => BattleTeam.Player; }
        public UnitData Data { get; private set; }
        
        public string DisplayName { get; private set; }

        public int GridX => Mathf.FloorToInt(_transform.position.x);
        public int GridY => Mathf.FloorToInt(_transform.position.z);

        public Vector2Int Location => Vector2Int.FloorToInt(Extensions.GameV3ToV2(_transform.position));

        private void Awake()
        {
            cpuUnitController = GetComponent<CpuUnitController>();
            Anim = GetComponent<UnitAnimation>();
            _transform = transform;
        }

        public void Initialize(UnitSO _unitSO, [CanBeNull] string name)
        {
            // setup SO values
            unitSO = _unitSO;
            DisplayName = name == null ? _unitSO.displayName : name;
            
            cpuUnitController.SetAI(unitSO.AI);
            
            Anim.Initialize(this, unitSO.AnimatorOverrideController);
            Anim.SwitchStateTo(UnitAnimation.Idle);

            Data = unitSO.Create();
            Data.Bind(this);
            
            Data.OnAttackHit += (info) => 
                Log($"{name} deal {info.damageModifier.value} damage to {info.target.name}");

            Stats.InitializeMaxValues();
            Stats.Evaluate();
        }

        public SkillSO[] GetSkills() => Data.Skills.ToArray();

        public void AddStatusEffect(StatusEffectRegister se)
        {
            Data.SEHandler.Register(se);
        }
        
        public void ReduceStatusEffect(StatusEffectId id)
        {
            Data.SEHandler.Reduce(id);
        }
        
        public void RemoveStatusEffect(StatusEffectId id)
        {
            Data.SEHandler.RemoveAll(id);
        }

        public void Attack(AttackInfo attackInfo) => Data.Attack(attackInfo);

        public void TakeAttack(AttackInfo attackInfo) => Data.TakeAttack(attackInfo);

        public void TakeDamage(DamageInfo damageInfo) => Data.TakeDamage(damageInfo);

        private void Log(string msg)
        {
            if (_logConsole == null) return;
            _logConsole.SendText(msg);
        }
        
        
    }
}