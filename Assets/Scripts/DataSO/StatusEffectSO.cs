using Game.DataSet;
using UnityEngine;

namespace Game.Unit.StatusEffect
{
    public abstract class StatusEffectSO1 : DataEntrySO<StatusEffectId>
    {
        [SerializeField] private Sprite _icon;
        public Sprite icon { get; }
        public abstract void RegisterTo(UnitObject unit);
        public abstract void RemoveFrom(UnitObject unit);
    }

    public enum StatusEffectId
    {
        None,
        AttackDamageUp,
        DamageBoost1,
        AttackDamageReduction,
        TurnPoison,
        MomentPoison,
        OneWay,
    }
}