using System;
using System.Collections.Generic;
using UnityEngine;

using Game.Unit.StatusEffect;

namespace Game.Unit.Ability
{
    [CreateAssetMenu(menuName = "Game/Ability/AB_DEBUG")]
    public class Ab_ConditionalBuff : AbilitySO
    {
        // [SerializeField] private Condition _condition;
        // [SerializeField] private BuffEntry _buff;

        public override void RegisterTo(UnitObject unit, UnitObject.UnitPartTree.UnitPartTreeNode node)
        {
            unit.seHandler.RegisterStatusEffects(new SE_DamageReduction(.99f, this));
        }
    }
    //
    // [Serializable]
    // public struct Condition
    // {
    //     public enum UnitValueType
    //     {
    //         HP,
    //         MP,
    //         HPPercent,
    //         Height,
    //         Constant_25,
    //         Constant_50,
    //         Constant_75,
    //         Constant_80,
    //         Constant_100,
    //     }
    //     
    //     public enum TriggerType
    //     {
    //         OnHPChanged,
    //         OnMPChanged,
    //         OnStartTakenAttack,
    //         OnStartTakenDamage,
    //         OnTakenAttack,
    //         OnTakenDamage,
    //         OnStartDealDamage,
    //         OnDealDamage,
    //     }
    //     
    //     public enum ComparisonType
    //     {
    //         Greater,
    //         Less,
    //         GreaterOrEquals,
    //         LessOrEquals,
    //         Equals,
    //         NotEquals,
    //     }
    //     
    //     [SerializeField] private UnitValueType _unitValueType;
    //     [SerializeField] private ComparisonType _comparisonType;
    //     [SerializeField] private UnitValueType _targetValueType;
    //     [SerializeField] private float _value;
    //
    //     private static Dictionary<UnitValueType, Func<UnitObject, float>> _valueLookUpTable = new Dictionary<UnitValueType, Func<UnitObject, float>>
    //     {
    //         {UnitValueType.HP, (unit) => unit.param.DUR},
    //         {UnitValueType.MP, (unit) => unit.param.MP},
    //         {UnitValueType.HPPercent, (unit) => unit.param.HPPercent},
    //         {UnitValueType.Height, (unit) => unit.height},
    //         {UnitValueType.Constant_25, (_) => .25f},
    //         {UnitValueType.Constant_50, (_) => .50f},
    //         {UnitValueType.Constant_75, (_) => .75f},
    //         {UnitValueType.Constant_80, (_) => .80f},
    //         {UnitValueType.Constant_100, (_) => .100f},
    //     };
    //     
    //     private static Dictionary<TriggerType, Action<UnitObject, Action<UnitObject>>> _triggerLookUpTable = new Dictionary<TriggerType, Action<UnitObject, Action<UnitObject>>>
    //     {
    //         {TriggerType.OnHPChanged, (unit, callback) => unit.param.OnHPChanged += callback},
    //         {TriggerType.OnMPChanged, (unit, callback) => unit.param.OnMPChanged += callback},
    //         {TriggerType.OnStartTakenAttack, (unit, callback) => unit.OnStartTakenAttack += callback},
    //         {TriggerType.OnStartTakenDamage, (unit, callback) => unit.OnStartTakenDamage += callback},
    //         {TriggerType.OnTakenAttack, (unit, callback) => unit.OnTakenAttack += callback},
    //         {TriggerType.OnTakenDamage, (unit, callback) => unit.OnTakenDamage += callback},
    //         {TriggerType.OnStartDealDamage, (unit, callback) => unit.OnStartDealDamage += callback},
    //         {TriggerType.OnDealDamage, (unit, callback) => unit.OnDealDamage += callback},
    //     };
    //     
    //     public bool MatchCondition(float inputValue)
    //     {
    //         return (inputValue > _value && _comparisonType == ComparisonType.Greater) ||
    //                (inputValue < _value && _comparisonType == ComparisonType.Less) ||
    //                (inputValue >= _value && _comparisonType == ComparisonType.GreaterOrEquals) ||
    //                (inputValue <= _value && _comparisonType == ComparisonType.LessOrEquals) ||
    //                (FloatComparer.AreEqual(inputValue, _value, 0.001f) && _comparisonType == ComparisonType.Equals) ||
    //                (!FloatComparer.AreEqual(inputValue, _value, 0.001f) && _comparisonType == ComparisonType.NotEquals);
    //     }
    // }
    //
    // [Serializable]
    // public struct BuffEntry
    // {
    //     public enum BuffType
    //     {
    //         DamageUp,
    //         DamageReduction,
    //         CriticalRateUp,
    //         DodgeRateUp,
    //     }
    //
    //     public static Dictionary<BuffType, Func<float, StatusEffect.StatusEffect>> _buffGenerateTable = new Dictionary<BuffType, Func<float, StatusEffect.StatusEffect>>()
    //     {
    //         {BuffType.DamageUp, (value) => new SE_AttackDamageUp(value)},
    //         {BuffType.DamageReduction, (value) => new SE_DamageReduction(value)},
    //         {BuffType.CriticalRateUp, (value) => new SE_DamageReduction(value)},
    //         {BuffType.DodgeRateUp, (value) => new SE_DamageReduction(value)},
    //     };
    //
    //     [SerializeField] private BuffType _buffType;
    //     [SerializeField] private int _turns;
    //     [SerializeField] private float _value;
    //     [SerializeField] private BaseStatModifier.ModifyType _modifyType;
    //     public BuffType buffType { get; }
    //     public BaseStatModifier.ModifyType modifyType { get; }
    //
    //     public StatusEffect.StatusEffect Buff => _buffGenerateTable[_buffType](_value);
    // }
}