using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Unit.Ability
{
    [CreateAssetMenu(menuName = "Game/Abilities/Ab_DamageReduction", fileName = "Ab_DamageReduction")]
    public class Ab_DamageReduction : AbilitySO
    {
        [SerializeField] private int _damageReduction;
        
        public override void RegisterTo(UnitObject unit, UnitObject.UnitPartTree.UnitPartTreeNode node)
        {
            unit.OnStartTakenDamage += ReduceDamageTaken;
        }

        private void ReduceDamageTaken(DamageInfo damageInfo)
        {
            damageInfo.damageStat.AddModifier(
                new DamageStatModifier(-_damageReduction, BaseStatModifier.ModifyType.Flat, null)
                );
        }
    }
}