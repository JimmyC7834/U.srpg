using UnityEngine;

namespace Game.Unit.Ability
{
    [CreateAssetMenu(menuName = "Game/Ability/Ab_DmgBuffOnFullHealth")]
    public class Ab_DmgBuffOnFullHealthSO : AbilitySO
    {
        [Header("Buff Settings")]
        [SerializeField] private ParamModifier.ModifyType _modifyType;
        [SerializeField] private float _modifyValue;

        public override UnitAbility Create(int count)
        {
            return new Ab_DmgBuffOnFullHealth(count,
                new DamageValueModifier(_modifyValue, _modifyType));
        }
    }

    public class Ab_DmgBuffOnFullHealth : UnitAbility
    {
        public override AbilityID ID { get => AbilityID.AttackDamageUpFullHpP; }
        private static DamageValueModifier _modifier;

        public Ab_DmgBuffOnFullHealth(int count, DamageValueModifier modifier)
            : base(count)
        {
            _modifier ??= modifier;
        }

        public override void OnPreAttack(AttackInfo info)
        {
            UnitObject foe = info.target;
            UnitObject self = info.source.unit;

            if (self.Stats.DurPercentage >= .99f)
            {
                info.AddModifier(_modifier);
            }
        }
    }
}