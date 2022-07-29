using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Unit
{
    [Serializable]
    public struct UnitParam
    {
        private static readonly float BASE_CRITRATE = 0.05f;
        private static readonly float BASE_DODGERATE = 0.05f;
        private static readonly float BASE_HITRATE = 0.95f;
        
        [SerializeField] private int _maxMP;
        [SerializeField] private int _maxHP;
        [SerializeField] private int _maxSan;

        [SerializeField] private BaseStat _hitRate;
        [SerializeField] private BaseStat _dodgeRate;
        [SerializeField] private BaseStat _critRate;

        [SerializeField] private UnitStat _dur;
        [SerializeField] private UnitStat _str;
        [SerializeField] private UnitStat _dex;
        [SerializeField] private UnitStat _tec;
        [SerializeField] private UnitStat _per;
        [SerializeField] private UnitStat _san;
        private UnitStat[] _param;
        private UnitObject _unit;

        public UnitParam Initialize(UnitObject unit)
        {
            _hitRate = new BaseStat();
            _dodgeRate = new BaseStat();
            _critRate = new BaseStat();
            
            _hitRate.AddModifier(new BaseStatModifier(BASE_HITRATE, BaseStatModifier.ModifyType.Flat));
            _dodgeRate.AddModifier(new BaseStatModifier(BASE_DODGERATE, BaseStatModifier.ModifyType.Flat));
            _critRate.AddModifier(new BaseStatModifier(BASE_CRITRATE, BaseStatModifier.ModifyType.Flat));
            
            _param = new []
            {
                _dur = new UnitStat(),
                _str = new UnitStat(),
                _dex = new UnitStat(),
                _tec = new UnitStat(),
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
        public int TEC { get => _tec.Value; }
        public int PER { get => _per.Value; }
        public int SAN { get => _san.Value; }
        
        public event Action<UnitObject> OnHPChanged;
        public event Action<UnitObject> OnMPChanged;
        public event Action<UnitObject> OnSANChanged;

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
            v = TEC;
            v = PER;
            v = SAN;
        }

        public bool CheckCritical() => Random.Range(0f, 1f) < _critRate.Value;
        public bool CheckDodge() => Random.Range(0f, 1f) < _dodgeRate.Value;
        public bool CheckHit() => Random.Range(0f, 1f) < _hitRate.Value;
        
    }
}