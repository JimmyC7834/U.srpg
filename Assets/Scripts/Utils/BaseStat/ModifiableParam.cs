using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Game
{
    /**
     * Mutable value only modifiable via ParamModifier
     */
    [Serializable]
    public class ModifiableParam
    {
        private float _baseValue;
        
        /**
         * public access to the value, recalculate value if modified 
         */
        public virtual float Value { get { return (dirty) ? Evaluate() : lastValue; } }
        [field: SerializeField] public float lastValue { get; private set; }
        protected bool dirty = true;
        protected readonly List<ParamModifier> modifiers;
        public readonly ReadOnlyCollection<ParamModifier> Modifiers;

        public ModifiableParam()
        {
            modifiers = new List<ParamModifier>();
            Modifiers = modifiers.AsReadOnly();
        }

        public ModifiableParam(float _value) : this()
        {
            _baseValue = _value;
            lastValue = _baseValue;
        }

        public virtual void AddModifier(ParamModifier modifier)
        {
            dirty = true;
            modifiers.Add(modifier);
            modifiers.Sort(CompareModifier);
        }
        
        protected virtual int CompareModifier(ParamModifier x, ParamModifier y) => 
            (x.priority < y.priority) ? -1 : (x.priority == y.priority) ? 0 : 1;

        public virtual bool RemoveModifier(ParamModifier modifier)
        {
            if (modifiers.Remove(modifier))
            {
                dirty = true;
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
                    dirty = true;
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
        
        /**
         * Recalculate value base on modifiers added
         */
        protected virtual float Evaluate()
        {
            float value = _baseValue;
            float percentSum = 0;
            for (int i = 0; i < modifiers.Count; i++)
            {
                switch (modifiers[i].type)
                {
                    case ParamModifier.ModifyType.Flat:
                        value += modifiers[i].value;
                        break;
                    case ParamModifier.ModifyType.PercentAdd:
                        percentSum += modifiers[i].value;
                        if (i + 1 >= modifiers.Count || modifiers[i + 1].priority > modifiers[i].priority)
                            value *= 1 + percentSum;
                        break;
                    case ParamModifier.ModifyType.Percent:
                        value *= modifiers[i].value;
                        break;
                }
            }

            lastValue = value;
            dirty = false;
            return (float) Math.Round(value, 2);
        }
    }
}