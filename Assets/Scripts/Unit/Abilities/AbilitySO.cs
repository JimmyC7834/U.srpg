using System;
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
        public abstract UnitAbility Create(int count);
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