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

        public SE_Poison(float _damagePerTurn)
        {
            damagePerTurn = _damagePerTurn;
        }
        
        protected override void Register(UnitObject unit)
        {
            unit.OnTurnChanged += DealDamage;
        }
    
        public override void Remove()
        {
            unit.OnTurnChanged -= DealDamage;
        }
    
        public void DealDamage(UnitObject unit)
        {
            DamageInfo damageInfo = DamageInfo.From(this);
            damageInfo.AddModifier(new DamageStatModifier(damagePerTurn, BaseStatModifier.ModifyType.Flat));
            unit.TakeDamage(damageInfo);
        }
    }
}