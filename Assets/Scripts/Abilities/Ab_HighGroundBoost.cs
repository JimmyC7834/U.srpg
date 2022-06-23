using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Unit.Ability
{
    [CreateAssetMenu(menuName = "Game/Abilities/Ab_HighGroundBoost", fileName = "Ab_HighGroundBoost")]
    public class Ab_HighGroundBoost : AbilitySO
    {
        [SerializeField] private float _triggerHeight;
        [SerializeField] private float _value;
        [SerializeField] private BaseStatModifier.ModifyType _modifyType;
        [SerializeField] private bool isAttackBoost;

        public override void RegisterTo(UnitObject unit, UnitObject.UnitPartTree.UnitPartTreeNode node)
        {
            if (isAttackBoost)
            {
                unit.OnStartDealDamage += BoostAttack;
            }
            else
            {
                unit.OnStartTakenAttack += BoostDefence;
            }
        }

        public void BoostAttack(AttackInfo attackInfo)
        {
            if (attackInfo.source.sourceUnit.height > attackInfo.target.height)
            {
                attackInfo.AddModifier(_modifierDict[_modifyType](_value));
            }
        }
        
        public void BoostDefence(AttackInfo attackInfo)
        {
            if (attackInfo.source.sourceUnit.height < attackInfo.target.height)
            {
                attackInfo.AddModifier(_modifierDict[_modifyType](-_value));
            }
        }
    }
}