using System;
using System.Collections;
using System.Collections.Generic;
using Game.Battle;
using Game.Unit;
using Game.Unit.StatusEffect;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

namespace Game.UI
{
    public class UI_UnitInfoSEPanel : UI_View
    {
        [SerializeField] private BattleService _battleService;
        [SerializeField] private GridLayoutGroup _gridLayout;

        [SerializeField] private List<UI_SEIndicator> _indicators;
        [SerializeField] private UI_SEIndicator _prefab;
        private GameObjectPool<UI_SEIndicator> _pool;

        protected override void Enter()
        {
            _pool = new GameObjectPool<UI_SEIndicator>(_prefab, _gridLayout.transform);
            _indicators = new List<UI_SEIndicator>();

            _battleService.cursor.OnTileChange += UpdatePanel;
        }

        private void UpdatePanel(CursorController _)
        {
            if (!_battleService.CurrentUnit) return;
            LoadRegisters(_battleService.CurrentUnit.seHandler.StatusEffects);
        }
        
        public void LoadRegisters(StatusEffect[] statusEffects)
        {
            Clear();
            foreach (StatusEffect statusEffect in statusEffects)
            {
                AddIndicator(statusEffect);
            }
        }

        private void AddIndicator(StatusEffect statusEffect)
        {
            UI_SEIndicator newIndicator = _pool.Get(obj => obj.Initialize(statusEffect));
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