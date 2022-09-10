using System;
using System.Collections;
using System.Collections.Generic;
using Game.Battle;
using Game.Unit;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

namespace Game.UI
{
    public class UI_UnitInfoSEPanel : MonoBehaviour
    {
        [SerializeField] private BattleService _battleService;
        [SerializeField] private GridLayoutGroup _gridLayout;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private List<UI_SEIndicator> _indicators;
        private ObjectPool<UI_SEIndicator> _pool;

        public void Initialize(BattleService battleService)
        {
            _battleService = battleService;
            _pool = new ObjectPool<UI_SEIndicator>(CreatSeIndicator, PoolItem, ReleaseItem);
            _indicators = new List<UI_SEIndicator>();

            _battleService.cursor.OnTileChange += UpdatePanel;
        }

        public UI_SEIndicator CreatSeIndicator() => Instantiate(_prefab).GetComponent<UI_SEIndicator>();
        
        private void PoolItem(UI_SEIndicator item)
        {
            item.gameObject.SetActive(true);
        }
        
        private void ReleaseItem(UI_SEIndicator item)
        {
            item.gameObject.SetActive(false);
        }

        private void UpdatePanel(CursorController _)
        {
            if (_battleService.CurrentUnitObject == null) return;
            LoadRegisters(_battleService.CurrentUnitObject.statusEffectRegisters);
        }
        
        public void LoadRegisters(List<StatusEffectRegister> registers)
        {
            Clear();
            foreach (StatusEffectRegister register in registers)
            {
                AddIndicator(register);
            }
        }

        private void AddIndicator(StatusEffectRegister register)
        {
            UI_SEIndicator newIndicator = _pool.Get();
            newIndicator.Initialize(register);
            newIndicator.transform.SetParent(_gridLayout.transform, false);
            _indicators.Add(newIndicator);
        }

        private void Clear()
        {
            foreach (UI_SEIndicator indicator in _indicators)
            {
                _pool.Release(indicator);
            }
            
            _indicators.Clear();
        }
    }
}