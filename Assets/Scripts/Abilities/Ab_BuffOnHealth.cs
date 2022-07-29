using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Unit.Ability
{
    [CreateAssetMenu(menuName = "Game/Ability/Ab_BuffOnHealth")]
    public class Ab_BuffOnHealth : AbilitySO
    {
        [Header("Trigger Settings")]
        [SerializeField] private Comparer _comparer;
        
        [Header("Buff Settings")]
        [SerializeField] private BuffType _buffType;
        [SerializeField] private BaseStatModifier.ModifyType _modifyType;
        [SerializeField] private float _modifyValue;

        public override void RegisterTo(UnitObject unit, UnitObject.UnitPartTree.UnitPartTreeNode node)
        {
            switch (_buffType)
            {
                case BuffType.DamageReduction:
                    unit.OnStartTakenAttack += DamageReductionTrigger;
                    break;
                case BuffType.DamageUp:
                    unit.OnStartTakenAttack += DamageUpTrigger;
                    break;
            }
        }

        public void DamageReductionTrigger(AttackInfo attackInfo)
        {
            if (_comparer.MatchCondition(attackInfo.target.param.HPPercent))
            {
                attackInfo.AddModifier(_modifierDict[_modifyType](_modifyValue));
            }
        }
        
        public void DamageUpTrigger(AttackInfo attackInfo)
        {
            if (_comparer.MatchCondition(attackInfo.source.sourceUnit.param.HPPercent))
            {
                attackInfo.AddModifier(_modifierDict[_modifyType](_modifyValue));
            }
        }
    }
}