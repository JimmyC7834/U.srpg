using Cinemachine;
using UnityEngine;
using Game.Battle.Map;
using Game.Unit;

namespace Game.Battle
{
    [CreateAssetMenu(menuName = "Game/Service/Battle")]
    public class BattleService : ScriptableObject
    {
        // ---MANAGERS GETTER---
        public UnitManager unitManager { get; private set; }
        public CursorController cursor  { get; private set; }
        public BattleUIManager battleUIManager { get; private set; }
        public BattleBoard battleBoard { get; private set; }
        public BattleTurnManager battleTurnManager { get; private set; }
        public MapHighlighter mapHighlighter { get; private set; }
        public DebugConsole debugConsole { get; private set; }
        public LogConsole logConsole { get; private set; }
        public CinemachineVirtualCamera camera { get; private set; }
        
        // Provide services
        public void ProvideUnitManager(UnitManager _unitManager) => unitManager = _unitManager;
        public void ProvideCursorController(CursorController _cursor) => cursor = _cursor;
        public void ProvideUIManager(BattleUIManager battleUIManager) => this.battleUIManager = battleUIManager;
        public void ProvideBattleBoard(BattleBoard _battleBoard) => battleBoard = _battleBoard;
        public void ProvideBattleTurnManager(BattleTurnManager _battleTurnManager) => battleTurnManager = _battleTurnManager;
        public void ProvideMapHighlighter(MapHighlighter _mapHighlighter) => mapHighlighter = _mapHighlighter;
        public void ProvideDebugConsole(DebugConsole _debugConsole) => debugConsole = _debugConsole;
        public void ProvideLogConsole(LogConsole _logConsole) => logConsole = _logConsole;
        public void ProvideCamera(CinemachineVirtualCamera _camera) => camera = _camera;

        public void InitializeDebugConsole()
        {
            debugConsole.AddItem("Koku", () => battleTurnManager.koku.ToString());
            debugConsole.AddItem("Turn", () => battleTurnManager.turn.ToString());
            debugConsole.AddItem("Current Selected Unit", () =>
            {
                if (battleBoard.ContainsCoord(cursor.MapCoord))
                    return CurrentUnit == null ? "null" : CurrentUnit.name;
                return "null";
            });
            debugConsole.AddItem("Current Selected Tile", () => 
            {
                if (battleBoard.ContainsCoord(cursor.MapCoord))
                    return CurrentTile.ToString();
                return "null";
            });
        }
        
        // Other services
        public UnitObject CurrentUnit => battleBoard.GetUnit(cursor.MapCoord);
        public BattleBoardTile CurrentTile => battleBoard.GetTile(cursor.MapCoord);
        public Vector2 CurrentCoord => cursor.MapCoord;
        public bool CanSelectCurrentUnit => CurrentUnit != null && 
                                            unitManager.GetCurrentUnits().Contains(CurrentUnit);
        public int CurrentKoku => battleTurnManager.koku;
        public int CurrentTurn => battleTurnManager.turn;
    }
}