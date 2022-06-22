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
                unit.OnStartTakenDamage += BoostDefence;
            }
        }

        public void BoostAttack(DamageInfo damageInfo)
        {
            if (damageInfo.source.sourceUnit.height > damageInfo.target.height)
            {
                damageInfo.AddModifier(_modifierDict[_modifyType](_value));
            }
        }
        
        public void BoostDefence(DamageInfo damageInfo)
        {
            if (damageInfo.source.sourceUnit.height < damageInfo.target.height)
            {
                damageInfo.AddModifier(_modifierDict[_modifyType](-_value));
            }
        }
    }
}