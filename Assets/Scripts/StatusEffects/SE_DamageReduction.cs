using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Unit.StatusEffect
{
    public class SE_DamageReduction : StatusEffect
    {
        [Tooltip("0f < 1f: percentage add reduction\n>= 1f: flat reduction\nfloor value for float > 1")]
        private float _value;

        public SE_DamageReduction(float value) => _value = value;

        protected override void Register(UnitObject unit)
        {
            unit.OnStartTakenAttack += ReduceDamage;
        }

        public override void Remove()
        {
            unit.OnStartTakenAttack -= ReduceDamage;
        }
        
        public void ReduceDamage(AttackInfo attackInfo)
        {
            if (_value <= 0)
            {
                Debug.LogError("DamageReduction <= 0 !!!!");
                return;
            }

            if (_value < 1f)
            {
                attackInfo.AddModifier(new DamageStatModifier(-_value, BaseStatModifier.ModifyType.PercentAdd));
                return;
            }
            
            attackInfo.AddModifier(new DamageStatModifier(-(int)_value, BaseStatModifier.ModifyType.Flat));
        }
    }
}