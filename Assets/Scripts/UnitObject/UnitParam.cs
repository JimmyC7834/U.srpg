using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Unit
{
    public enum UnitStatType
    {
        DUR,
        STR,
        DEX,
        PER,
        SAN,
        Count,
    }
    
    [Serializable]
    public class UnitParam : ModifiableParam
    {
        public UnitParam() : base() { }
        public UnitParam(int baseValue) : base(baseValue) { }
        
        /**
         * Override to floor the value
         */
        public override float Value { get => (int) base.Value; }

        /**
         * Used by PartSO for param boost entry
         */
        [Serializable]
        public struct ParamBoostEntry
        {
            public UnitStatType unitStatType;
            public int value;
            public UnitStatModifier ToModifier() => 
                new UnitStatModifier(unitStatType, value, ParamModifier.ModifyType.Flat, null);
        }
    }
    
    public class UnitStatModifier : ParamModifier
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
    
    /**
     * A modifiable param object for damage calculation
     */
    public class DamageValue : ModifiableParam
    {
        /**
         * Round down value to integer and clip the value [0, ~]
         */
        public int Value { get => Mathf.Max(0, Mathf.RoundToInt(base.Value)); }
    }
    
    /**
     * Modifier for DamageStat
     */
    public class DamageValueModifier : ParamModifier
    {
        public DamageValueModifier(float _value, ModifyType _type, int _priority) :
            base(_value, _type, _priority, null) { }
        
        public DamageValueModifier(float _value, ModifyType _type) :
            this(_value, _type, (int)_type) { }
    }
    
}