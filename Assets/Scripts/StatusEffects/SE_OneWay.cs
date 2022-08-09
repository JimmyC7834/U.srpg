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
            unit.OnEndingAction += HandleActionEnd;
            unit.OnTurnChanged += RecoverMP;
        }

        public void HandleActionEnd(UnitObject _)
        {
            _mpLeft = unit.param.MP;
            if (_mpLeft < 0)
            {
                unit.RemoveStatusEffect(this);
            }
            unit.param.ChangeMP(-_mpLeft);
        }

        public void RecoverMP(UnitObject _)
        {
            unit.param.ChangeMP(_mpLeft);
        }
        
        public override void Remove()
        {
            unit.OnEndingAction -= HandleActionEnd;
            unit.OnTurnChanged -= RecoverMP;
        }
    }
}