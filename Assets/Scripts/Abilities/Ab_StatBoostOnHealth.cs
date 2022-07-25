using Game.Unit.StatusEffect;
using UnityEngine;

namespace Game.Unit.Ability
{
    [CreateAssetMenu(menuName = "Game/Abilities/Ab_StatBoostOnHealth", fileName = "Ab_StatBoostOnHealth")]
    public class Ab_StatBoostOnHealth : AbilitySO
    {
        [SerializeField] private float _triggerHealth;
        [SerializeField] private float _value;
        [SerializeField] private UnitStatModifier.ModifyType _modifyType;
        [SerializeField] private bool isAttackBoost;

        public override void RegisterTo(UnitObject unit, UnitObject.UnitPartTree.UnitPartTreeNode node)
        {
            if (isAttackBoost)
            {
                unit.param.OnHPChanged += BoostAttack;
            }
            else
            {
                unit.param.OnHPChanged += BoostDefence;
            }
        }

        public void BoostAttack(UnitObject unit)
        {
            if (unit.param.DUR < unit.param.MaxHP / 2)
            {
                unit.RegisterStatusEffects(StatusEffectId.DamageBoost1);
                return;
            }
            
            unit.RemoveStatusEffects(StatusEffectId.DamageBoost1);
        }
        
        public void BoostDefence(UnitObject unit)
        {
            // if (damageInfo.source.sourceUnit.height > damageInfo.target.height)
            // {
            //     damageInfo.AddModifier(
            //         new DamageStatModifier(
            //             (_modifyType == BaseStatModifier.ModifyType.Flat) ? -_value : 1 - _value,
            //             _modifyType, null)
            //     );
            // }
        }
    }
}