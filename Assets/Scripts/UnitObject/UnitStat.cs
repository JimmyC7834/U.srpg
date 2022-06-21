using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Unit
{
    [Serializable]
    public class UnitStat : BaseStat
    {
        // round down
        public int Value { get => (int) base.Value; }
        protected override float Evaluate()
        {
            base.Evaluate();
            lastValue = (int) lastValue;
            return lastValue;
        }
        
        [Serializable]
        public struct StatBoostEntry
        {
            public UnitStatType unitStatType;
            public int value;
            public UnitStatModifier ToModifier() => new UnitStatModifier(unitStatType, value, BaseStatModifier.ModifyType.Flat, null);
        }
    }
    
    public class UnitStatModifier : BaseStatModifier
    {
        public readonly UnitStatType statType;

        public UnitStatModifier(UnitStatType _statType, float _value, ModifyType _type, int _priority, object _source) : 
            base(_value, _type, _priority, _source)
        {
            statType = _statType;
        }
        
        public UnitStatModifier(UnitStatType _statType, float _value, ModifyType _type, object _source) : 
            this(_statType, _value, _type, (int)_type, _source) { }
    }

    public class DamageStat : BaseStat
    {
        public int Value { get => Mathf.Max(0, Mathf.RoundToInt(base.Value)); }
    }
    
    public class DamageStatModifier : BaseStatModifier
    {
        public DamageStatModifier(float _value, ModifyType _type, int _priority, object _source) :
            base(_value, _type, _priority, _source) { }
        
        public DamageStatModifier(float _value, ModifyType _type, object _source) :
            this(_value, _type, (int)_type, _source) { }
    }
    
}