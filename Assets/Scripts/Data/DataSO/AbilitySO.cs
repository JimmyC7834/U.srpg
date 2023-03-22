using System;
using System.Collections.Generic;
using System.Linq;
using Game.DataSet;
using UnityEngine;

namespace Game.Unit.Ability
{
    /// <summary>
    /// Base class for metadata and constructor function of Abilities
    /// </summary>
    public abstract class AbilitySO : DataEntrySO<AbilityID>
    {
        /// <summary>
        /// Constructor function of Ability objects. Derived class should
        /// Properly create and initialize the ability object before returning it.
        /// </summary>
        /// <param name="unit"> The unit that is getting the ability </param>
        /// <param name="count"> The count of the ability </param>
        /// <returns></returns>
        public abstract Ability Create(UnitObject unit, int count);
    }

    /// <summary>
    /// Represents a ability of an unit. The ability could be stacked
    /// and operate base on the number of stacks.
    /// 
    /// This is a base class for any ability class to be inherited.
    /// This base class provides various functions invoked on event calls
    /// which the derived class should override as needed.
    /// </summary>
    public abstract class Ability : IUnitEventsListener, IDataId<AbilityID>
    {
        /// <summary>
        /// The derived class should define this its corresponding id
        /// </summary>
        public abstract AbilityID ID { get; }
        /// <summary>
        /// The count of ability stack.
        /// </summary>
        public int Count { get; private set; }
        /// <summary>
        /// The unit object this ability is attached to.
        /// This variable is initialized on construct.
        /// </summary>
        protected readonly UnitObject _unit;

        protected Ability(UnitObject unit, int count)
        {
            _unit = unit;
            Count = count;
        }

        /// <summary>
        /// Increase the count of the ability stack.
        /// </summary>
        /// <param name="value"></param>
        public void Stack(int value)
        {
            Count += value;
        }
        
        private void OnRegister() { }
        private void OnRemove() { }
        protected void OnStack() { }
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