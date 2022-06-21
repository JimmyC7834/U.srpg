using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Unit
{
    [Serializable]
    public struct UnitParam
    {
        [SerializeField] private int _maxDur;
        [SerializeField] private int _maxSan;
        [SerializeField] private UnitStat _dur;
        [SerializeField] private UnitStat _str;
        [SerializeField] private UnitStat _dex;
        [SerializeField] private UnitStat _per;
        [SerializeField] private UnitStat _san;
        private UnitStat[] _param;
        private UnitObject _unit;

        public UnitParam Initialize(UnitObject unit)
        {
            _param = new []
            {
                _dur = new UnitStat(),
                _str = new UnitStat(),
                _dex = new UnitStat(),
                _per = new UnitStat(),
                _san = new UnitStat(),
            };

            _unit = unit;
            _maxDur = DUR;
            _maxSan = SAN;
            return this;
        }

        public void InitializeMaxValues()
        {
            _maxDur = DUR;
            _maxSan = SAN;
        }

        private UnitStat this[UnitStatType statType]
        {
            get => _param[(int) statType];
        }
            
        public int MaxDUR { get => _maxDur; }
        public int DUR { get => _dur.Value; }
        public int STR { get => _str.Value; }
        public int DEX { get => _dex.Value; }
        public int PER { get => _per.Value; }
        public int SAN { get => _san.Value; }
        
        public event Action<UnitObject> OnDurChanged;

        public void AddModifier(UnitStatModifier modifier)
        {
            this[modifier.statType].AddModifier(modifier);
            if (modifier.statType == UnitStatType.DUR)
            {
                OnDurChanged?.Invoke(_unit);
            }
        }

        public void AddModifiers(IEnumerable<UnitStatModifier> modifiers)
        {
            foreach (UnitStatModifier modifier in modifiers)
            {
                AddModifier(modifier);
            }
        }

        public void Evaluate()
        {
            int v = DUR;
            v = STR;
            v = DEX;
            v = PER;
            v = SAN;
        }
    }
}