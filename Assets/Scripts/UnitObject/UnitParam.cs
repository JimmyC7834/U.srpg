using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Unit
{
    [Serializable]
    public struct UnitParam
    {
        [SerializeField] private int _maxMP;
        [SerializeField] private int _maxHP;
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
            MP = 10;
            
            _maxHP = DUR;
            _maxSan = SAN;
            return this;
        }

        public void InitializeMaxValues()
        {
            _maxHP = DUR;
            _maxSan = SAN;
        }

        private UnitStat this[UnitStatType statType]
        {
            get => _param[(int) statType];
        }
            
        public int MaxHP { get => _maxHP; }
        public float HPPercent { get => DUR/(float) _maxHP; }
        public int MP { get; private set; }
        public int DUR { get => _dur.Value; }
        public int STR { get => _str.Value; }
        public int DEX { get => _dex.Value; }
        public int PER { get => _per.Value; }
        public int SAN { get => _san.Value; }
        
        public event Action<UnitObject> OnHPChanged;
        public event Action<UnitObject> OnMPChanged;

        public void AddModifier(UnitStatModifier modifier)
        {
            this[modifier.statType].AddModifier(modifier);
            if (modifier.statType == UnitStatType.DUR)
            {
                OnHPChanged?.Invoke(_unit);
            }
        }

        public void AddModifiers(IEnumerable<UnitStatModifier> modifiers)
        {
            foreach (UnitStatModifier modifier in modifiers)
            {
                AddModifier(modifier);
            }
        }

        public void ResetMP()
        {
            MP = _maxMP;
            OnMPChanged?.Invoke(_unit);
        }
        
        public void ConsumeMp(int value)
        {
            MP -= value;
            OnMPChanged?.Invoke(_unit);
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