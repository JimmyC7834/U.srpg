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
        [SerializeField] private BattleMap _battleMap;
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
            _battleData._mapHighlighter = _mapHighlighter;
            _battleData._battleMap = _battleMap;
            
            _unitManager.Initialize(_battleSO.unitSpawnInfos);
            _battlePhraseManager.Initialize();
            _battleMap.Initialize(_battleSO);
        }
    }
}
