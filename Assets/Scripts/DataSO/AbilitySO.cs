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
        public enum UnitValueType
        {
            HP,
            MP,
            HPPercent,
            Height,
            Constant_25,
            Constant_50,
            Constant_75,
            Constant_80,
            Constant_100,
        }
        
        public enum ComparisonType
        {
            Greater,
            Less,
            GreaterOrEquals,
            LessOrEquals,
            Equals,
            NotEquals,
        }
        
        [SerializeField] private UnitValueType _unitValueType;
        [SerializeField] private ComparisonType _comparisonType;
        [SerializeField] private UnitValueType _targetValueType;
        [SerializeField] private float _value;

        private static Dictionary<UnitValueType, Func<UnitObject, float>> _valueLookUpTable = new Dictionary<UnitValueType, Func<UnitObject, float>>
        {
            {UnitValueType.HP, (unit) => unit.param.DUR},
            {UnitValueType.MP, (unit) => unit.param.MP},
            {UnitValueType.HPPercent, (unit) => unit.param.HPPercent},
            {UnitValueType.Height, (unit) => unit.height},
            {UnitValueType.Constant_25, (_) => .25f},
            {UnitValueType.Constant_50, (_) => .50f},
            {UnitValueType.Constant_80, (_) => .80f},
            {UnitValueType.Constant_100, (_) => .100f},
        };
        
        public bool MatchCondition(float inputValue)
        {
            return (inputValue > _value && _comparisonType == ComparisonType.Greater) ||
                   (inputValue < _value && _comparisonType == ComparisonType.Less) ||
                   (inputValue >= _value && _comparisonType == ComparisonType.GreaterOrEquals) ||
                   (inputValue <= _value && _comparisonType == ComparisonType.LessOrEquals) ||
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