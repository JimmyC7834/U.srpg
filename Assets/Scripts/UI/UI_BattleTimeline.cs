using System;
using System.Collections;
using System.Collections.Generic;
using Game.Battle;
using Game.Unit;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UI_BattleTimeline : MonoBehaviour
    {
        [SerializeField] private BattleService _battleService;
        
        private static readonly int TOTAL_KOKU = 25;
        [SerializeField] private HorizontalLayoutGroup[] _horizontalLayouts;
        [SerializeField] private Slider _kokuIndicator;
        
        public HorizontalLayoutGroup[] horizontalLayouts { get => _horizontalLayouts; }

        private void Start()
        {
            _battleService.battleTurnManager.OnKokuChanged += UpdateKokuIndicator;
        }

        private void UpdateKokuIndicator(int koku)
        {
            _kokuIndicator.value = 20 - koku;
        }
    }
}