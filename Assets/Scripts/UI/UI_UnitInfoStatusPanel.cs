using System;
using System.Collections;
using System.Collections.Generic;
using Game.Battle;
using Game.Unit;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Game.UI
{
    public class UI_UnitInfoStatusPanel : UI_View
    {
        [SerializeField] private BattleService _battleService;
        [SerializeField] private TMP_Text _displayNameLabel;
        [SerializeField] private Image _hpBar;
        [SerializeField] private Image _icon;

        protected override void Enter()
        {
            _battleService.cursor.OnTileChange += UpdatePanel;
        }

        private void UpdatePanel(CursorController _)
        {
            UnitObject unit = _battleService.CurrentUnitObject;
            if (unit == null) return;
            
            _displayNameLabel.SetText(unit.displayName);
            _hpBar.fillAmount = unit.param.HPPercent;
            _icon.sprite = unit.unitSO.sprite;
        }
    }
}