using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.Unit.StatusEffect
{
    public class SE_HitRateUp : StatusEffect
    {
        [Tooltip("0f <= 1f: percentage add")]
        private float _value;

        public SE_HitRateUp(float value, ScriptableObject source) : base(source)
        {
            _value = value;
        }
        
        protected override void Register()
        {
            unit.stats.ModifyHitRate(_value, this);
        }

        protected override void OnRemoval()
        {
            if (unit.Equals(null)) return;
            unit.stats.RemoveHitRateModifier(this);
        }
    }
    
    public class SE_HitRateDown : SE_HitRateUp
    {
        [Tooltip("0f <= 1f: percentage minus")]
        private float _value;

        public SE_HitRateDown(float value, ScriptableObject source) : base(-value, source) { }
    }
}
