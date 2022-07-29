using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Unit.StatusEffect
{
    public class SE_AttackDamageUp : StatusEffect
    {
        [Tooltip("0f < 1f: percentage add reduction\n>= 1f: flat reduction\nfloor value for float > 1")]
        private float _value;

        public SE_AttackDamageUp(float value)
        {
            _value = value;
        }
        
        protected override void Register(UnitObject _unit)
        {
            unit.OnStartDealDamage += BoostAttack;
        }

        public override void Remove()
        {
            if (unit.Equals(null)) return;
            unit.OnStartDealDamage -= BoostAttack;
        }
        
        public void BoostAttack(AttackInfo attackInfo)
        {
            if (_value <= 0)
            {
                Debug.LogError("DamageUp <= 0 !!!!");
                return;
            }

            if (_value < 1f)
            {
                attackInfo.AddModifier(new DamageStatModifier(_value, BaseStatModifier.ModifyType.PercentAdd));
                return;
            }
            
            attackInfo.AddModifier(new DamageStatModifier((int)_value, BaseStatModifier.ModifyType.Flat));
        }
    }
}
