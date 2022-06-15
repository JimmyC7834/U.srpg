using System;
using System.Collections;
using System.Collections.Generic;
using Game.Battle.Map;
using UnityEngine;

namespace Game.Battle
{
    public class BattleManager : MonoBehaviour
    {
        [SerializeField] private BattleSO _battleSO;
        [SerializeField] private BattleData _battleData;
        
        [SerializeField] private CursorController _cursor;
        [SerializeField] private UnitManager _unitManager;
        [SerializeField] private UIManager _uiManager;
        [SerializeField] private BattlePhraseManager _battlePhraseManager;
        private BattleBoard _battleBoard;
        [SerializeField] private MapHighlighter _mapHighlighter;

        private void OnEnable()
        {
            if (_battleSO == null || _battleData == null)
            {
                Debug.LogError("Null battle SO or Data!");
                return;
            }

            _battleData.cursor = _cursor;
            
            _battleData.unitManager = _unitManager;
            _battleData.uiManager = _uiManager;
            _battleData.mapHighlighter = _mapHighlighter;
            _battleBoard = new BattleBoard(_battleSO);
            _battleData.battleBoard = _battleBoard;
            
            _unitManager.Initialize(_battleSO.unitSpawnInfos);
            _battlePhraseManager.Initialize();
        }
    }
}
