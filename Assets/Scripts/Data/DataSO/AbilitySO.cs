using System;
using System.Collections.Generic;
using System.Linq;
using Game.DataSet;
using UnityEngine;

namespace Game.Unit.Ability
{
    public abstract class AbilitySO : DataEntrySO<AbilityId>
    {
        protected Dictionary<ParamModifier.ModifyType, Func<float, DamageValueModifier>> _modifierDict = new ()
        {
            {ParamModifier.ModifyType.Flat, (value) => 
                new DamageValueModifier(value, ParamModifier.ModifyType.Flat)},
            {ParamModifier.ModifyType.Percent, (value) => 
                new DamageValueModifier(1 + value, ParamModifier.ModifyType.Percent)},
            {ParamModifier.ModifyType.PercentAdd, (value) => 
                new DamageValueModifier(value, ParamModifier.ModifyType.PercentAdd)},
        };
        
        public abstract void RegisterTo(UnitObject unit, UnitObject.UnitPartTree.UnitPartTreeNode node);
    }

    public enum AbilityId
    {
        None = -1,
        DamageReduction1 = 10,
        AttackDamageUpOnHighGroundF = 20,
        AttackDamageUpOnHighGroundP = 30,
        DamageReductionUpOnHighGroundF = 40,
        DamageReductionUpOnHighGroundP = 50,
        AttackDamageAndDamageReductionUpOnHighGroundP = 60,
        AttackDamageUpFullHpP = 70,
        AttackDamageUpOver75HpP = 80,
        AttackDamageUpUnder50HpP= 90,
        DamageReductionFullHpP = 100,
        ReduceAP = 110,
        IncreaseAP = 120,
        AB_DEBUG = 9999,
        Count = 11,
    }

    public abstract class Ability : IUnitEventsListener, IDataId<AbilityId>
    {
        public abstract AbilityId ID { get; }
        public int Count { get; private set; }
        
        protected readonly UnitObject _unit;

        protected Ability(UnitObject unit, int count)
        {
            _unit = unit;
            Count = count;
        }

        public void Stack(int value)
        {
            Count += value;
        }
        
        private void OnRegister() { }
        private void OnRemove() { }
        public virtual void OnActionStart() { }
        public virtual void OnActionEnd() { }
        public virtual void OnTurnStart() { }
        public virtual void OnTurnEnd() { }
        public virtual void OnMomentStart() { }
        public virtual void OnMomentEnd() { }
        public virtual void OnPreAttack(AttackInfo info) { }
        public virtual void OnPostAttack(AttackInfo info) { }
        public virtual void OnAttackMissed(AttackInfo info) { }
        public virtual void OnAttackDodged(AttackInfo info) { }
        public virtual void OnAttackHit(AttackInfo info) { }
        public virtual void OnPreTakeAttack(AttackInfo info) { }
        public virtual void OnPostTakeAttack(AttackInfo info) { }
        public virtual void OnDodgeAttack(AttackInfo info) { }
        public virtual void OnPreTakeDamage(DamageInfo info) { }
        public virtual void OnPostTakeDamage(DamageInfo info) { }
    }

    public class AbilityHandler
    {
        private Dictionary<AbilityId, Ability> _abilities;

        public AbilityHandler(UnitObject unit)
        {
            unit.OnPreAttack += (info) => Map((x) => x.OnPreAttack(info));
            unit.OnPostAttack += (info) => Map((x) => x.OnPostAttack(info));
            unit.OnAttackMissed += (info) => Map((x) => x.OnAttackMissed(info));
            unit.OnAttackDodged += (info) => Map((x) => x.OnAttackDodged(info));
            unit.OnAttackHit += (info) => Map((x) => x.OnAttackHit(info));
            unit.OnPreTakeAttack += (info) => Map((x) => x.OnPreTakeAttack(info));
            unit.OnPostTakeAttack += (info) => Map((x) => x.OnPostTakeAttack(info));
            unit.OnDodgedAttack += (info) => Map((x) => x.OnDodgeAttack(info));
            unit.OnPreTakeDamage += (info) => Map((x) => x.OnPreTakeDamage(info));
            unit.OnPostTakeDamage += (info) => Map((x) => x.OnPostTakeDamage(info));
        }
        
        public void Register(Ability ability)
        {
            if (_abilities.ContainsKey(ability.ID))
            {
                _abilities[ability.ID].Stack(ability.Count);
                return;
            }
            
            _abilities.Add(ability.ID, ability);
        }

        public void Reduce(Ability ability)
        {
            if (!_abilities.ContainsKey(ability.ID)) return;
            _abilities[ability.ID].Stack(-ability.Count);
            if (_abilities[ability.ID].Count <= 0)
                RemoveAll(ability.ID);
        }
        
        public void RemoveAll(AbilityId id)
        {
            if (!_abilities.ContainsKey(id)) return;
            _abilities.Remove(id);
        }

        private void Map(Action<Ability> map)
        {
            foreach (Ability ability in _abilities.Values)
                map(ability);
        }

        public Ability[] GetAbilities() => _abilities.Values.ToArray();
    }

    [Serializable]
    public struct Comparer
    {
        public enum CompareType
        {
            Greater,
            Less,
            GreaterOrEquals,
            LessOrEquals,
        }
        
        [SerializeField] private CompareType _compareType;
        [SerializeField] private float _value;

        public static Comparer From(CompareType compareType, float value) => new Comparer()
        {
            _compareType = compareType,
            _value = value,
        };
        
        public bool MatchCondition(float inputValue)
        {
            return (inputValue > _value && _compareType == CompareType.Greater) ||
                   (inputValue < _value && _compareType == CompareType.Less) ||
                   (inputValue >= _value && _compareType == CompareType.GreaterOrEquals) ||
                   (inputValue <= _value && _compareType == CompareType.LessOrEquals);
        }
    }

    public enum BuffType
    {
        DamageUp,
        DamageReduction,
        CirtRateUp,
        DodgeRateUp,
        HitRateUp,
    }
}