using System;
using System.Collections;
using System.Collections.Generic;
using Game.Battle.Map;
using UnityEngine;

namespace Game.Battle
{
    // TODO: Camera control
    // TODO: Part Destroy
    // TODO: Part Disable System
    
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
            _battleService.ProvideLogConsole(GetComponent<LogConsole>());
        }

        private void Start()
        {
            // initialize all units
            _unitManager.Initialize(_battleSO.unitSpawnInfos);
            // initialize abilities/ status effects on turn and koku
            _battleTurnManager.Initialize();
            
            // ~ Battle Start
            _battlePhraseManager.Initialize(_battleService);

            _battleService.InitializeDebugConsole();
        }
    }
}
