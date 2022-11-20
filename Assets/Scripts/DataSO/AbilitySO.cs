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