using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Unit
{
    public class UnitParam : MonoBehaviour
    {
        public enum UnitStatType
        {
            DUR,
            STR,
            DEX,
            PER,
            SAN,
            Count,
        }
        
        [SerializeField] private UnitStat _dur;
        [SerializeField] private UnitStat _str;
        [SerializeField] private UnitStat _dex;
        [SerializeField] private UnitStat _per;
        [SerializeField] private UnitStat _san;
        private UnitStat[] _param;

        public UnitParam Initialize(UnitSO data)
        {
            _param = new []
            {
                _dur = new UnitStat(),
                _str = new UnitStat(),
                _dex = new UnitStat(),
                _per = new UnitStat(),
                _san = new UnitStat(),
            };

            return this;
        }

        private UnitStat this[UnitStatType statType]
        {
            get => _param[(int) statType];
        }
            
        
        public int DUR { get => _dur.Value; }
        public int STR { get => _str.Value; }
        public int DEX { get => _dex.Value; }
        public int PER { get => _per.Value; }
        public int SAN { get => _san.Value; }

        public void AddModifier(UnitStatModifier modifier)
        {
            this[modifier.statType].AddModifier(modifier);
        }

        public void AddModifiers(UnitStatModifier[] modifiers)
        {
            for (int i = 0; i < modifiers.Length; i++)
            {
                AddModifier(modifiers[i]);
            }
        }
    }
}