using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

namespace Game.Unit.StatusEffect
{
    public abstract class StatusEffect
    {
        public enum EffectType { Stackable, Duration, CountDown, Special }

        public static readonly int MAX_COUNT = 5;

        public static readonly Dictionary<Type, StatusEffectId> IDMAP = new Dictionary<Type, StatusEffectId>()
        {
            {typeof(SE_Poison), StatusEffectId.Poison1},
        };

        public static readonly Dictionary<Type, EffectType> TYPEMAP = new Dictionary<Type, EffectType>()
        {
            {typeof(SE_Poison), EffectType.CountDown},
            {typeof(SE_OneWay), EffectType.Special},
            {typeof(SE_DamageReduction), EffectType.Duration},
        };
        
        protected Sprite _icon;
        
        public Sprite icon { get; }
        public StatusEffectId id { get; }
        public ScriptableObject source { get; }
        public EffectType seType { get; }
        public Tuple<ScriptableObject, Type> key { get; }
        
        public UnitObject unit { get; protected set; }
        public int count { get; protected set; }

        public StatusEffect(ScriptableObject _source)
        {
            // id = IDMAP[GetType()];
            seType = TYPEMAP[GetType()];
            source = _source;
            key = UnitSEHandler.CreateKey(this);
        }
        
        public void RegisterTo(UnitObject _unit)
        {
            // prevent reassign by accident
            if (unit) return;
            unit = _unit;
            unit.OnSETurnChanged += CountDown;
            if (seType == EffectType.Special)
                count = -1;
            
            Register();
        }

        public void CountDown(UnitObject _)
        {
            if (!unit) return;
            if (seType != EffectType.Special)
            {
                count--;
                if (count == 0)
                    Remove();
            }

            OnCountDown();
        }

        public void Remove()
        {
            if (unit.Equals(null)) return;
            unit.seHandler.RemoveSE(this);
            OnRemoval();
        }

        public void Stack(int _count)
        {
            if (seType == EffectType.Special) return;
            count = Mathf.Min(count + _count, MAX_COUNT);
            OnStacked();
        }

        protected virtual void Register() { }

        protected virtual void OnCountDown() { }
        protected virtual void OnStacked() { }

        protected virtual void OnRemoval() { }
    }

    public class SE_MPRegenUp : StatusEffect
    {
        private int _value;

        public SE_MPRegenUp(int value, ScriptableObject source) : base(source)
        {
            _value = value;
        }

        protected override void OnCountDown() => unit.param.ChangeAP(_value);
    }

    public class SE_MPRegenDown : SE_MPRegenUp
    {
        public SE_MPRegenDown(int value, ScriptableObject source) : base(-value, source) { }
    }
    
    public class SE_HPRegenPerTurn : StatusEffect
    {
        [Tooltip("0f < 1f: percentage add \n>= 1f: flat \nfloor value for float > 1")]
        private float _value;

        public SE_HPRegenPerTurn(float value, ScriptableObject source) : base(source)
        {
            _value = value;
        }
        
        protected override void OnCountDown() => IncreaseHP();

        private void IncreaseHP()
        {
            if (Mathf.Abs(_value) < 1f)
            {
                unit.param.AddModifier(new UnitStatModifier(
                    UnitStatType.DUR, _value, BaseStatModifier.ModifyType.PercentAdd, this));
                return;
            }
            
            unit.param.AddModifier(new UnitStatModifier(
                UnitStatType.DUR, (int) _value, BaseStatModifier.ModifyType.Flat, this));
        }
    }
    
    public class SE_MOVUp : StatusEffect
    {
        [Tooltip("0f < 1f: percentage add \n>= 1f: flat \nfloor value for float > 1")]
        private float _value;

        public SE_MOVUp(int value, ScriptableObject source) : base(source)
        {
            _value = value;
        }
        
        protected override void Register()
        {
            unit.param.ModifyMOV(_value, this);
        }

        protected override void OnRemoval()
        {
            unit.param.RemoveMOVRateModifier(this);
        }
    }

    public class SE_MOVDown : SE_MOVUp
    {
        public SE_MOVDown(int value, ScriptableObject source) : base(-value, source) { }
    }
    
    public class SE_MPConsumeUp : StatusEffect
    {
        private int _value;

        public SE_MPConsumeUp(int value, ScriptableObject source) : base(source)
        {
            _value = value;
        }
        
        protected override void Register()
        {
            unit.param.OnAPConsumed += ReduceAP;
        }

        protected override void OnRemoval()
        {
            unit.param.OnAPConsumed -= ReduceAP;
        }

        private void ReduceAP(UnitObject _)
        {
            unit.param.ChangeAP(_value);
        }
    }
}