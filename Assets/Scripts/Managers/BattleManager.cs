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
        [SerializeField] private BattleService _battleService;
        
        [SerializeField] private CursorController _cursor;
        [SerializeField] private UnitManager _unitManager;
        [SerializeField] private UIManager _uiManager;
        [SerializeField] private BattleTurnManager _battleTurnManager;
        [SerializeField] private BattlePhraseManager _battlePhraseManager;
        [SerializeField] private MapHighlighter _mapHighlighter;

        private void OnEnable()
        {
            if (_battleSO == null || _battleService == null)
            {
                Debug.LogError("Null battle SO or Data!");
                return;
            }

            _battleService.ProvideCursorController(_cursor);
            _battleService.ProvideUnitManager(_unitManager);
            _battleService.ProvideUIManager(_uiManager);
            _battleService.ProvideMapHighlighter(_mapHighlighter);
            _battleService.ProvideBattleTurnManager(_battleTurnManager);
            _battleService.ProvideBattleBoard(new BattleBoard(_battleSO));
            _battleService.ProvideDebugConsole(GetComponent<DebugConsole>());
        }

        private void Start()
        {
            _unitManager.Initialize(_battleSO.unitSpawnInfos);
            _battlePhraseManager.Initialize(_battleService);
        }
    }
}
