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
        
        protected Dictionary<BaseStatModifier.ModifyType, Func<float, DamageStatModifier>> _modifierDict = new ()
        {
            {BaseStatModifier.ModifyType.Flat, (value) => 
                new DamageStatModifier(value, BaseStatModifier.ModifyType.Flat)},
            {BaseStatModifier.ModifyType.Percent, (value) => 
                new DamageStatModifier(1 + value, BaseStatModifier.ModifyType.Percent)},
            {BaseStatModifier.ModifyType.PercentAdd, (value) => 
                new DamageStatModifier(value, BaseStatModifier.ModifyType.PercentAdd)},
        };
        
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
        None = -1,
        DamageReduction1 = 10,
        HighGroundAttack = 20,
        HighGroundPush = 30,
        HighGroundBlock = 40,
        HighGroundDefence = 50,
        HighGroundAdvantage = 60,
        AttackUpUnder50Hp = 70,
        AttackUpFullHp = 80,
        AttackUpOver80Hp = 90,
        DamageReductionFullHp = 100,
        DamageReductionOver80Hp = 110,
        Count = 11,
    }
}