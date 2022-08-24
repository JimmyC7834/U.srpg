using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Unit.StatusEffect
{
    public class SE_OneWay : StatusEffect
    {
        private int _mpLeft;
        
        protected override void Register()
        {
            unit.OnSEActionEnd += HandleActionEnd;
            unit.OnSETurnChanged += RecoverMP;
        }

        public void HandleActionEnd(UnitObject _)
        {
            _mpLeft = unit.param.AP;
            if (_mpLeft < 0)
            {
                unit.RemoveStatusEffect(this);
            }
            unit.param.ChangeAP(-_mpLeft);
        }

        public void RecoverMP(UnitObject _)
        {
            unit.param.ChangeAP(_mpLeft);
        }
        
        public override void Remove()
        {
            unit.OnSEActionEnd -= HandleActionEnd;
            unit.OnSETurnChanged -= RecoverMP;
        }
    }
}