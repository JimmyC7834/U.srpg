using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Game.Unit;
using Game.Unit.StatusEffect;
using UnityEngine;

namespace Game
{
    public class UnitSEHandler
    {
        private UnitObject _unit;
        private Dictionary<Tuple<ScriptableObject, Type>, StatusEffect> _registers;

        public StatusEffect[] StatusEffects => _registers.Values.ToArray();
        
        public UnitSEHandler(UnitObject unit)
        {
            _unit = unit;
            _registers = new Dictionary<Tuple<ScriptableObject, Type>, StatusEffect>();
        }
        
        public void RegisterStatusEffects(StatusEffect statusEffect)
        {
            if (_registers.ContainsKey(statusEffect.key))
            {
                _registers[statusEffect.key].Stack(statusEffect.count);
                return;
            }
            
            _registers.Add(statusEffect.key, statusEffect);
            statusEffect.RegisterTo(_unit);
        }

        public void RemoveSE(StatusEffect statusEffect)
        {
            Tuple<ScriptableObject, Type> key = statusEffect.key;
            if (!_registers.ContainsKey(key)) return;
            _registers.Remove(key);
        }

        public void RemoveSEBySource(ScriptableObject source)
        {
            List<Tuple<ScriptableObject, Type>> regs = 
                _registers.Keys.Where(reg => reg.Item1.Equals(source)).ToList();
            foreach (Tuple<ScriptableObject,Type> reg in regs)
            {
                _registers.Remove(reg);
            }
        }

        public static Tuple<ScriptableObject, Type> CreateKey(StatusEffect statusEffect) =>
            new Tuple<ScriptableObject, Type>(statusEffect.source, statusEffect.GetType());
    }
}
 