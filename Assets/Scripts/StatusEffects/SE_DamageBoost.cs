using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Unit.StatusEffects
{
    [CreateAssetMenu(menuName = "Game/StatusEffects/SE_DamageBoost")]
    public class SE_DamageBoost : StatusEffectSO
    {
        public int _value;
        
        public override void RegisterTo(UnitObject unit)
        {
            unit.OnStartDealDamage += BoostAttack;
        }

        public override void RemoveFrom(UnitObject unit)
        {
            unit.OnStartDealDamage -= BoostAttack;
        }
        
        public void BoostAttack(DamageInfo damageInfo)
        {
            damageInfo.AddModifier(new DamageStatModifier(_value, BaseStatModifier.ModifyType.Flat, this));
        }
    }
}
