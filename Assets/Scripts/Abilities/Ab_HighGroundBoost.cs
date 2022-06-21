using UnityEngine;

namespace Game.Unit.Ability
{
    [CreateAssetMenu(menuName = "Game/Abilities/Ab_HighGroundBoost", fileName = "Ab_HighGroundBoost")]
    public class Ab_HighGroundBoost : AbilitySO
    {
        [SerializeField] private float _triggerHeight;
        [SerializeField] private float _value;
        [SerializeField] private UnitStatModifier.ModifyType _modifyType;
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
                damageInfo.AddModifier(new DamageStatModifier(
                    (_modifyType == BaseStatModifier.ModifyType.Flat) ? _value : 1 + _value, 
                    _modifyType, null));
            }
        }
        
        public void BoostDefence(DamageInfo damageInfo)
        {
            if (damageInfo.source.sourceUnit.height > damageInfo.target.height)
            {
                damageInfo.AddModifier(
                    new DamageStatModifier(
                        (_modifyType == BaseStatModifier.ModifyType.Flat) ? -_value : 1 - _value,
                        _modifyType, null)
                    );
            }
        }
    }
}