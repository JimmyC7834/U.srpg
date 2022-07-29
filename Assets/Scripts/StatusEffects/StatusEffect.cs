using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Unit.StatusEffect
{
    public abstract class StatusEffect
    {
        [SerializeField] protected Sprite _icon;
        public Sprite icon { get; }
        public UnitObject unit { get; protected set; }

        public void RegisterTo(UnitObject _unit)
        {
            if (unit != null) return;
            unit = _unit;
            Register(_unit);
        }

        protected abstract void Register(UnitObject _unit);

        public abstract void Remove();
    }
}