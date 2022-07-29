using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.Unit.Ability
{
    [CreateAssetMenu(menuName = "Game/Ability/Ab_Ab_BuffOnHighGround")]
    public class Ab_BuffOnHighGround : AbilitySO
    {
        public static float _triggerValue;
        
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
            UnitObject self = attackInfo.target;
            UnitObject foe = attackInfo.source.sourceUnit;
            
            if (self.height - foe.height >= _triggerValue)
            {
                attackInfo.AddModifier(_modifierDict[_modifyType](_modifyValue));
            }
        }
        
        public void DamageUpTrigger(AttackInfo attackInfo)
        {
            UnitObject foe = attackInfo.target;
            UnitObject self = attackInfo.source.sourceUnit;
            
            if (self.height - foe.height >= _triggerValue)
            {
                attackInfo.AddModifier(_modifierDict[_modifyType](_modifyValue));
            }
        }
    }
}
