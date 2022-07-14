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
            unit.OnStartTakenAttack += ReduceAttackTaken;
        }

        private void ReduceAttackTaken(AttackInfo attackInfo)
        {
            attackInfo.AddModifier(
                new DamageStatModifier(-_damageReduction, BaseStatModifier.ModifyType.Flat));
        }
    }
}