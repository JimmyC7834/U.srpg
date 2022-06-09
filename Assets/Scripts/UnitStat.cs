using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Unit
{
    public class UnitStat : BaseStat
    {
        // round down
        public int Value { get => (int) base.Value; }
    }

    public class UnitStatModifier : BaseStatModifier
    {
        public readonly UnitParam.UnitStatType statType;

        public UnitStatModifier(UnitParam.UnitStatType _statType, float _value, ModifyType _type, int _priority, object _source) : 
            base(_value, _type, _priority, _source)
        {
            statType = _statType;
        }
        
        public UnitStatModifier(UnitParam.UnitStatType _statType, float _value, ModifyType _type, object _source) : 
            this(_statType, _value, _type, (int)_type, _source) { }
    }
}