using System.Collections;
using System.Collections.Generic;
using Game.Battle;
using Game.Battle.Map;
using UnityEngine;

namespace Game.Unit.StatusEffect
{
    public class SE_Poison : StatusEffect
    {
        [SerializeField] private float damagePerTurn;

        public SE_Poison(float _damagePerTurn, int _turns, ScriptableObject source) : base(source)
        {
            damagePerTurn = _damagePerTurn;
        }

        protected override void OnCountDown() => DealDamage();

        public void DealDamage()
        {
            DamageInfo damageInfo = DamageInfo.From(this);
            damageInfo.AddModifier(new DamageStatModifier(damagePerTurn, BaseStatModifier.ModifyType.Flat));
            unit.TakeDamage(damageInfo);
        }
    }
    
    public class SE_KokuPoison : StatusEffect
    {
        [SerializeField] private float damagePerTurn;

        public SE_KokuPoison(float _damagePerTurn, int _turns, ScriptableObject source) : base(source)
        {
            damagePerTurn = _damagePerTurn;
        }
        
        protected override void Register()
        {
            unit.OnSEKokuChanged += DealDamage;
        }
    
        protected override void OnRemoval()
        {
            unit.OnSEKokuChanged -= DealDamage;
        }
    
        public void DealDamage(UnitObject unit)
        {
            DamageInfo damageInfo = DamageInfo.From(this);
            damageInfo.AddModifier(new DamageStatModifier(damagePerTurn, BaseStatModifier.ModifyType.Flat));
            unit.TakeDamage(damageInfo);
        }
    }
}