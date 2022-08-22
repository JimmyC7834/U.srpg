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
        
        protected override void Register()
        {
            unit.OnAbStartDealDamage += BoostAttack;
        }

        public override void Remove()
        {
            if (unit.Equals(null)) return;
            unit.OnAbStartDealDamage -= BoostAttack;
        }
        
        public void BoostAttack(AttackInfo attackInfo)
        {
            if (Mathf.Abs(_value) < 1f)
            {
                attackInfo.AddModifier(new DamageStatModifier(_value, BaseStatModifier.ModifyType.PercentAdd));
                return;
            }
            
            attackInfo.AddModifier(new DamageStatModifier((int)_value, BaseStatModifier.ModifyType.Flat));
        }
    }

    public class SE_AttackDamageDown : SE_AttackDamageUp
    {
        public SE_AttackDamageDown(float value) : base(-value) { }
    }

}
