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
        private static readonly int BASE_MOVERANGE = 9;
        private static readonly int DEFAULT_MP = 10;
        
        [SerializeField] private int _maxMP;
        [SerializeField] private int _maxHP;
        [SerializeField] private int _maxSan;

        [SerializeField] private BaseStat _hitRate;
        [SerializeField] private BaseStat _dodgeRate;
        [SerializeField] private BaseStat _critRate;
        [SerializeField] private BaseStat _counterRate;

        [SerializeField] private UnitStat _mov;
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
            _hitRate = new BaseStat(BASE_HITRATE);
            _dodgeRate = new BaseStat(BASE_DODGERATE);
            _critRate = new BaseStat(BASE_CRITRATE);
            _counterRate = new BaseStat(0f);
            _mov = new UnitStat(BASE_MOVERANGE);

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
            MP = DEFAULT_MP;
            
            _maxHP = DUR;
            _maxMP = DEFAULT_MP;
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
        public int MP;
        public int MOV { get => _mov.Value; }
        public int DUR { get => _dur.Value; }
        public int STR { get => _str.Value; }
        public int DEX { get => _dex.Value; }
        public int TEC { get => _tec.Value; }
        public int PER { get => _per.Value; }
        public int SAN { get => _san.Value; }
        
        public event Action<UnitObject> OnHPChanged;
        public event Action<UnitObject> OnMPChanged;
        public event Action<UnitObject> OnMPConsumed;
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
            OnMPConsumed?.Invoke(_unit);
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

        public void ChangeMP(int dv)
        {
            MP += dv;
            OnMPChanged?.Invoke(_unit);
        }
        
        public bool CheckCritical() => Random.Range(0f, 1f) < _critRate.Value;
        public bool CheckDodge() => Random.Range(0f, 1f) < _dodgeRate.Value;
        public bool CheckHit() => Random.Range(0f, 1f) < _hitRate.Value;
        public int GetMoveRange() => _mov.Value;

        public void ModifyHitRate(float _value, object _source) =>
            _hitRate.AddModifier(new BaseStatModifier(_value, BaseStatModifier.ModifyType.Flat, _source));
        public void RemoveHitRateModifier(object _source) => _hitRate.RemoveAllModifiersFromSource(_source);
        
        public void ModifyDodgeRate(float _value, object _source) =>
            _dodgeRate.AddModifier(new BaseStatModifier(_value, BaseStatModifier.ModifyType.Flat, _source));
        public void RemoveDodgeRateModifier(object _source) => _dodgeRate.RemoveAllModifiersFromSource(_source);
        
        public void ModifyCritRate(float _value, object _source) =>
            _critRate.AddModifier(new BaseStatModifier(_value, BaseStatModifier.ModifyType.Flat, _source));
        public void RemoveCritRateModifier(object _source) => _critRate.RemoveAllModifiersFromSource(_source);
        
        public void ModifyCounterRate(float _value, object _source) =>
            _counterRate.AddModifier(new BaseStatModifier(_value, BaseStatModifier.ModifyType.Flat, _source));
        public void RemoveCounterRateModifier(object _source) => _counterRate.RemoveAllModifiersFromSource(_source);

        public void ModifyMOV(float _value, object _source) =>
            _mov.AddModifier(new BaseStatModifier(_value, BaseStatModifier.ModifyType.Flat, _source));
        public void RemoveMOVRateModifier(object _source) => _mov.RemoveAllModifiersFromSource(_source);
    }
}