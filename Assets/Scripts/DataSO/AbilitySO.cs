using System;
using System.Collections;
using System.Collections.Generic;
using Game.DataSet;
using UnityEngine;

namespace Game.Unit.Ability
{
    public class AbilitySO : DataEntrySO<AbilityId>
    {
        public Sprite icon;
        
        public virtual void RegisterTo(UnitObject unit, UnitObject.UnitPartTree.UnitPartTreeNode node) { }
        public virtual void OnTrigger() { }
    }
    
    public enum AbilityId
    {
        DurIncrease1,
        StrIncrease1,
        DexIncrease1,
        PerIncrease1,
        SanIncrease1,
        Count,
    }
}