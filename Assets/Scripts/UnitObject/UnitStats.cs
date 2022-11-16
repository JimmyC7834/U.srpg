using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Unit
{
    /**
     * Mutable collection of params represent the stats of a unit
     */
    [Serializable]
    public class UnitStats
    {
        private static readonly float BASE_CRITRATE = 0.05f;
        private static readonly float BASE_DODGERATE = 0.05f;
        private static readonly float BASE_HITRATE = 0.95f;
        private static readonly int BASE_MOVERANGE = 9;
        private static readonly int DEFAULT_AP = 10;
        
        [SerializeField] private int _maxAP;
        [SerializeField] private int _maxDur;
        [SerializeField] private int _maxSan;

        [SerializeField] private ModifiableParam _hitRate;
        [SerializeField] private ModifiableParam _dodgeRate;
        [SerializeField] private ModifiableParam _critRate;
        [SerializeField] private ModifiableParam _counterRate;

        [SerializeField] private UnitParam _mov;
        [SerializeField] private UnitParam _dur;
        [SerializeField] private UnitParam _str;
        [SerializeField] private UnitParam _dex;
        [SerializeField] private UnitParam _tec;
        [SerializeField] private UnitParam _per;
        [SerializeField] private UnitParam _san;
        private UnitParam[] _param;
        private readonly UnitObject _unit;

        public float DurPercentage { get => DUR/(float) _maxDur; }
        public int AP { get; private set; }
        public int MOV { get => (int) _mov.Value; }
        public int DUR { get => (int) _dur.Value; }
        public int STR { get => (int) _str.Value; }
        public int DEX { get => (int) _dex.Value; }
        public int TEC { get => (int) _tec.Value; }
        public int PER { get => (int) _per.Value; }
        public int SAN { get => (int) _san.Value; }
        
        public event Action<UnitObject> OnHPChanged;
        public event Action<UnitObject> OnAPChanged;
        public event Action<UnitObject> OnAPConsumed;
        public event Action<UnitObject> OnSANChanged;

        
        public UnitStats(UnitObject unit)
        {
            _hitRate = new ModifiableParam(BASE_HITRATE);
            _dodgeRate = new ModifiableParam(BASE_DODGERATE);
            _critRate = new ModifiableParam(BASE_CRITRATE);
            _counterRate = new ModifiableParam(0f);
            _mov = new UnitParam(BASE_MOVERANGE);

            _param = new []
            {
                _dur = new UnitParam(),
                _str = new UnitParam(),
                _dex = new UnitParam(),
                _tec = new UnitParam(),
                _per = new UnitParam(),
                _san = new UnitParam(),
            };

            _unit = unit;
            AP = DEFAULT_AP;
            
            _maxDur = DUR;
            _maxAP = DEFAULT_AP;
            _maxSan = SAN;
        }

        public void InitializeMaxValues()
        {
            _maxDur = DUR;
            _maxSan = SAN;
        }

        private UnitParam this[UnitStatType statType]
        {
            get => _param[(int) statType];
        }
        
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
                AddModifier(modifier);
        }

        public void ResetAP()
        {
            AP = _maxAP;
            OnAPChanged?.Invoke(_unit);
        }
        
        public void ConsumeAP(int value)
        {
            AP -= value;
            OnAPChanged?.Invoke(_unit);
            OnAPConsumed?.Invoke(_unit);
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

        public void ChangeAP(int dv)
        {
            AP += dv;
            OnAPChanged?.Invoke(_unit);
        }
        
        public bool CheckCritical() => Random.Range(0f, 1f) < _critRate.Value;
        public bool CheckDodge() => Random.Range(0f, 1f) < _dodgeRate.Value;
        public bool CheckHit() => Random.Range(0f, 1f) < _hitRate.Value;
        public int GetMoveRange() => MOV;

        public void ModifyHitRate(float _value, object _source) =>
            _hitRate.AddModifier(new ParamModifier(_value, ParamModifier.ModifyType.Flat, _source));
        public void RemoveHitRateModifier(object _source) => _hitRate.RemoveAllModifiersFromSource(_source);
        
        public void ModifyDodgeRate(float _value, object _source) =>
            _dodgeRate.AddModifier(new ParamModifier(_value, ParamModifier.ModifyType.Flat, _source));
        public void RemoveDodgeRateModifier(object _source) => _dodgeRate.RemoveAllModifiersFromSource(_source);
        
        public void ModifyCritRate(float _value, object _source) =>
            _critRate.AddModifier(new ParamModifier(_value, ParamModifier.ModifyType.Flat, _source));
        public void RemoveCritRateModifier(object _source) => _critRate.RemoveAllModifiersFromSource(_source);
        
        public void ModifyCounterRate(float _value, object _source) =>
            _counterRate.AddModifier(new ParamModifier(_value, ParamModifier.ModifyType.Flat, _source));
        public void RemoveCounterRateModifier(object _source) => _counterRate.RemoveAllModifiersFromSource(_source);

        public void ModifyMOV(float _value, object _source) =>
            _mov.AddModifier(new ParamModifier(_value, ParamModifier.ModifyType.Flat, _source));
        public void RemoveMOVRateModifier(object _source) => _mov.RemoveAllModifiersFromSource(_source);
    }
}