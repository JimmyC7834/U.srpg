using UnityEngine;

namespace Game.Unit.Ability
{
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
                unit.unitParam.OnDurChanged += BoostAttack;
            }
            else
            {
                unit.unitParam.OnDurChanged += BoostDefence;
            }
        }

        public void BoostAttack(UnitParam param)
        {
            // DamageStatModifier damageModifier = new DamageStatModifier(
            //     (_modifyType == BaseStatModifier.ModifyType.Flat) ? _value : 1 + _value,
            //     _modifyType, null);
            //
            // if (param.DUR >= param.MaxDUR/2)
            // {
            //     damageInfo.AddModifier();
            // }
        }
        
        public void BoostDefence(UnitParam param)
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