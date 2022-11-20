using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Unit.StatusEffect
{
    public class SE_DamageReduction : StatusEffect
    {
        [Tooltip("0f < 1f: percentage add reduction\n>= 1f: flat reduction\nfloor value for float > 1")]
        private float _value;

        public SE_DamageReduction(float value, ScriptableObject source) : base(source) => _value = value;

        protected override void Register()
        {
            unit.OnSETakeAttack += ReduceDamage;
        }

        protected override void OnRemoval()
        {
            unit.OnSETakeAttack -= ReduceDamage;
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
                attackInfo.AddModifier(new DamageValueModifier(-_value, ParamModifier.ModifyType.PercentAdd));
                return;
            }
            
            attackInfo.AddModifier(new DamageValueModifier(-(int)_value, ParamModifier.ModifyType.Flat));
        }
    }
}