using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Unit.StatusEffect
{
    public abstract class StatusEffect
    {
        [SerializeField] protected Sprite _icon;
        public Sprite icon { get; }
        public UnitObject unit { get; protected set; }

        public void RegisterTo(UnitObject _unit)
        {
            if (unit != null) return;
            unit = _unit;
            Register();
        }

        protected abstract void Register();

        public abstract void Remove();
    }

    public class SE_MPRegenUp : StatusEffect
    {
        private int _value;

        public SE_MPRegenUp(int value)
        {
            _value = value;
        }
        
        protected override void Register()
        {
            unit.OnTurnChanged += IncreaseMP;
        }

        public override void Remove()
        {
            unit.OnTurnChanged -= IncreaseMP;
        }

        private void IncreaseMP(UnitObject _)
        {
            unit.param.ChangeAP(_value);
        }
    }

    public class SE_MPRegenDown : SE_MPRegenUp
    {
        public SE_MPRegenDown(int value) : base(-value) { }
    }
    
    public class SE_HPRegenPerTurn : StatusEffect
    {
        [Tooltip("0f < 1f: percentage add \n>= 1f: flat \nfloor value for float > 1")]
        private float _value;

        public SE_HPRegenPerTurn(float value)
        {
            _value = value;
        }
        
        protected override void Register()
        {
            unit.OnTurnChanged += IncreaseHP;
        }

        public override void Remove()
        {
            unit.OnTurnChanged -= IncreaseHP;
        }

        private void IncreaseHP(UnitObject _)
        {
            if (Mathf.Abs(_value) < 1f)
            {
                unit.param.AddModifier(new UnitStatModifier(UnitStatType.DUR, _value, BaseStatModifier.ModifyType.PercentAdd, this));
                return;
            }
            
            unit.param.AddModifier(new UnitStatModifier(UnitStatType.DUR, (int) _value, BaseStatModifier.ModifyType.Flat, this));
        }
    }
    
    public class SE_MOVUp : StatusEffect
    {
        [Tooltip("0f < 1f: percentage add \n>= 1f: flat \nfloor value for float > 1")]
        private float _value;

        public SE_MOVUp(int value)
        {
            _value = value;
        }
        
        protected override void Register()
        {
            unit.param.ModifyMOV(_value, this);
        }

        public override void Remove()
        {
            unit.param.RemoveMOVRateModifier(this);
        }
    }

    public class SE_MOVDown : SE_MOVUp
    {
        public SE_MOVDown(int value) : base(-value) { }
    }
    
    public class SE_MPConsumeUp : StatusEffect
    {
        private int _value;

        public SE_MPConsumeUp(int value)
        {
            _value = value;
        }
        
        protected override void Register()
        {
            unit.param.OnAPConsumed += ReduceAP;
        }

        public override void Remove()
        {
            unit.param.OnAPConsumed -= ReduceAP;
        }

        private void ReduceAP(UnitObject _)
        {
            unit.param.ChangeAP(_value);
        }
    }
}