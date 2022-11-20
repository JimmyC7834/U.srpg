using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Unit.StatusEffect
{
    public class SE_OneWay : StatusEffect
    {
        private int _mpLeft;

        public SE_OneWay(ScriptableObject source) : base(source) { }
        
        protected override void Register()
        {
            unit.OnSEActionEnd += HandleActionEnd;
        }

        public void HandleActionEnd(UnitObject _)
        {
            _mpLeft = unit.stats.AP;
            if (_mpLeft < 0)
            {
                Remove();
            }
            unit.stats.ChangeAP(-_mpLeft);
        }

        protected override void OnCountDown() => RecoverMP();

        public void RecoverMP()
        {
            unit.stats.ChangeAP(_mpLeft);
        }
        
        protected override void OnRemoval()
        {
            unit.OnSEActionEnd -= HandleActionEnd;
        }
    }
}