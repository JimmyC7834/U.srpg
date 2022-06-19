using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Game
{
    public class BaseStat
    {
        public float baseValue;
        public virtual float Value { get { return (changed) ? Evaluate() : lastValue; } }
        public float lastValue;
        protected bool changed = true;
        protected readonly List<BaseStatModifier> modifiers;
        public readonly ReadOnlyCollection<BaseStatModifier> Modifiers;

        public BaseStat()
        {
            modifiers = new List<BaseStatModifier>();
            Modifiers = modifiers.AsReadOnly();
        }

        public BaseStat(float _value) : this()
        {
            baseValue = _value;
            lastValue = baseValue;
        }

        public virtual void AddModifier(BaseStatModifier modifier)
        {
            changed = true;
            modifiers.Add(modifier);
            modifiers.Sort(CompareModifier);
        }

        protected virtual int CompareModifier(BaseStatModifier x, BaseStatModifier y) => (x.priority < y.priority) ? -1 : (x.priority == y.priority) ? 0 : 1;

        public virtual bool RemoveModifier(BaseStatModifier modifier)
        {
            if (modifiers.Remove(modifier))
            {
                changed = true;
                return true;
            }
            return false;
        }

        public virtual bool RemoveAllModifiersFromSource(object source)
        {
            bool removed = false;
            for (int i = modifiers.Count; i >= 0; i--)
            {
                if (modifiers[i].source == source)
                {
                    changed = true;
                    removed = true;
                    modifiers.RemoveAt(i);
                }
            }
            return removed;
        }

        public virtual bool RemoveAllModifiers()
        {
            if (modifiers.Count == 0) return false;
            modifiers.Clear();
            return true;
        }

        protected virtual float Evaluate()
        {
            float value = baseValue;
            float percentSum = 0;
            for (int i = 0; i < modifiers.Count; i++)
            {
                switch (modifiers[i].type)
                {
                    case BaseStatModifier.ModifyType.Flat:
                        value += modifiers[i].value;
                        break;
                    case BaseStatModifier.ModifyType.PercentAdd:
                        percentSum += modifiers[i].value;
                        if (i + 1 >= modifiers.Count || modifiers[i + 1].priority > modifiers[i].priority)
                            value *= 1 + percentSum;
                        break;
                    case BaseStatModifier.ModifyType.Percent:
                        value *= modifiers[i].value;
                        break;
                }
            }

            lastValue = value;
            changed = false;
            return (float) Math.Round(value, 2);
        }
    }
}