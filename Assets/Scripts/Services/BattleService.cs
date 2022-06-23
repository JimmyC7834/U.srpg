using System.Collections;
using System.Collections.Generic;
using Game.Battle.Map;
using Game.Unit;
using Game.Unit.Skill;
using UnityEngine;


namespace Game.Battle
{
    [CreateAssetMenu(menuName = "Game/Battle/Service")]
    public class BattleService : ScriptableObject
    {
        // ---MANAGERS GETTER---
        public UnitManager unitManager { get; private set; }
        public CursorController cursor  { get; private set; }
        public UIManager uiManager { get; private set; }
        public BattleBoard battleBoard { get; private set; }
        public BattleTurnManager battleTurnManager { get; private set; }
        public MapHighlighter mapHighlighter { get; private set; }
        
        // Provide services
        public void ProvideUnitManager(UnitManager _unitManager) => unitManager = _unitManager;
        public void ProvideCursorController(CursorController _cursor) => cursor = _cursor;
        public void ProvideUIManager(UIManager _uiManager) => uiManager = _uiManager;
        public void ProvideBattleBoard(BattleBoard _battleBoard) => battleBoard = _battleBoard;
        public void ProvideBattleTurnManager(BattleTurnManager _battleTurnManager) => battleTurnManager = _battleTurnManager;
        public void ProvideMapHighlighter(MapHighlighter _mapHighlighter) => mapHighlighter = _mapHighlighter;
        
        // Other services
        public UnitObject CurrentUnitObject => battleBoard.GetUnit(cursor.MapCoord);
        public BattleBoardTile CurrentTile => battleBoard.GetTile(cursor.MapCoord);
        public Vector2 CurrentCoord => cursor.MapCoord;
    }
}