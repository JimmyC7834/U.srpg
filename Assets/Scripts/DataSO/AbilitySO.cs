using System;
using System.Collections;
using System.Collections.Generic;
using Game.DataSet;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

namespace Game.Unit.Ability
{
    public abstract class AbilitySO : DataEntrySO<AbilityId>
    {
        public Sprite icon;

        public abstract void RegisterTo(UnitObject unit, UnitObject.UnitPartTree.UnitPartTreeNode node);
    }
    
    [Serializable]
    public struct Condition
    {
        public enum PlayerValueType
        {
            Height,
            DUR,
        }
        
        public enum ComparisonType
        {
            Greater,
            Less,
            Equals,
            NotEquals,
        }

        [SerializeField] private ComparisonType _comparisonType;
        [SerializeField] private float _value;

        public bool MatchCondition(float inputValue)
        {
            return (inputValue > _value && _comparisonType == ComparisonType.Greater) ||
                   (inputValue < _value && _comparisonType == ComparisonType.Less) ||
                   (FloatComparer.AreEqual(inputValue, _value, 0.001f) && _comparisonType == ComparisonType.Equals) ||
                   (!FloatComparer.AreEqual(inputValue, _value, 0.001f) && _comparisonType == ComparisonType.NotEquals);
        }
    }
    
    public enum AbilityId
    {
        None,
        DamageReduction1,
        HighGroundAttack,
        HighGroundPush,
        HighGroundBlock,
        HighGroundDefence,
        HighGroundAdvantage,
        AttackUpUnder50Hp,
        AttackUpFullHp,
        AttackUpOver80Hp,
        DamageReductionFullHp,
        DamageReductionOver80Hp,
        Count,
    }
}