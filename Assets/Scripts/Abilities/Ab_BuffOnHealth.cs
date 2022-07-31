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
                case BuffType.HitRateUp:
                    unit.OnInitiatingAttack += HitRateUpTrigger;
                    break;
                case BuffType.DodgeRateUp:
                    unit.OnInitiatingAttack += DodgeRateUpTrigger;
                    break;
                case BuffType.CirtRateUp:
                    unit.OnInitiatingAttack += CritRateUpTrigger;
                    break;
            }
        }

        public void DamageReductionTrigger(AttackInfo attackInfo)
        {
            UnitObject self = attackInfo.target;
            UnitObject foe = attackInfo.source.sourceUnit;
            
            if (_comparer.MatchCondition(self.param.HPPercent))
            {
                attackInfo.AddModifier(_modifierDict[_modifyType](_modifyValue));
            }
        }
        
        public void DamageUpTrigger(AttackInfo attackInfo)
        {
            UnitObject foe = attackInfo.target;
            UnitObject self = attackInfo.source.sourceUnit;

            if (_comparer.MatchCondition(self.param.HPPercent))
            {
                attackInfo.AddModifier(_modifierDict[_modifyType](_modifyValue));
            }
        }
        
        public void HitRateUpTrigger(AttackInfo attackInfo)
        {
            UnitObject foe = attackInfo.target;
            UnitObject self = attackInfo.source.sourceUnit;
            
            if (_comparer.MatchCondition(self.param.HPPercent))
            {
                self.param.ModifyHitRate(_modifyValue, this);
            }
        }
        
        public void DodgeRateUpTrigger(AttackInfo attackInfo)
        {
            UnitObject self = attackInfo.target;
            UnitObject foe = attackInfo.source.sourceUnit;
            
            if (_comparer.MatchCondition(self.param.HPPercent))
            {
                self.param.ModifyDodgeRate(_modifyValue, this);
            }
        }
        
        public void CritRateUpTrigger(AttackInfo attackInfo)
        {
            UnitObject foe = attackInfo.target;
            UnitObject self = attackInfo.source.sourceUnit;
            
            if (_comparer.MatchCondition(self.param.HPPercent))
            {
                self.param.ModifyCritRate(_modifyValue, this);
            }
        }
    }
}