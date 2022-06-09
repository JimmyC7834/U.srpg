using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Unit.Ability
{
    public enum AbilityId
    {
        DurIncrease1,
        StrIncrease1,
        DexIncrease1,
        PerIncrease1,
        SanIncrease1,
        Count,
    }
    
    public enum StatModifierId
    {
        DurIncrease1,
        StrIncrease1,
        DexIncrease1,
        PerIncrease1,
        SanIncrease1,
        Count,
    }
    
    public interface IStatModifier
    {
        public UnitStat PreprocessStat(UnitStat stat);
        public UnitStat CalculateStat(UnitStat stat);
    }
    
    public abstract class Ability
    {
        // public abstract 
    }
}