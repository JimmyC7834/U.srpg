using System.Collections;
using System.Collections.Generic;
using Game.Battle;
using Game.Battle.Map;
using UnityEngine;

namespace Game.Unit.StatusEffect
{
    // [CreateAssetMenu(menuName = "Game/StatusEffects/SE_Poison")]
    // public class SE_Poison : StatusEffectSO
    // {
    //     [SerializeField] private int damagePerTurn;
    //     
    //     public override void RegisterTo(UnitObject unit)
    //     {
    //         unit.OnTurnChanged += DealDamage;
    //     }
    //
    //     public override void RemoveFrom(UnitObject unit)
    //     {
    //         unit.OnTurnChanged -= DealDamage;
    //     }
    //
    //     public void DealDamage(UnitObject unit)
    //     {
    //         DamageInfo damageInfo = DamageInfo.From(this);
    //         damageInfo.AddModifier(new DamageStatModifier(damagePerTurn, BaseStatModifier.ModifyType.Flat));
    //         unit.TakeDamage(damageInfo);
    //     }
    // }
}