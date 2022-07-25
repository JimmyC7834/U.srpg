using System.Collections;
using System.Collections.Generic;
using Game.DataSet;
using UnityEngine;

namespace Game.Unit.StatusEffects
{
    public abstract class StatusEffectSO : DataEntrySO<StatusEffectId>
    {
        [SerializeField] private Sprite _icon;
        public Sprite icon { get; }
        public abstract void RegisterTo(UnitObject unit);
        public abstract void RemoveFrom(UnitObject unit);
    }

    public enum StatusEffectId
    {
        None = -1,
        DamageBoost1 = 10,
        Poison1 = 20,
        Count = 2,
    }
}