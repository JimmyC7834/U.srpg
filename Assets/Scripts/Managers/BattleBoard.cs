using System.Collections.Generic;
using Game.Unit;
using UnityEngine;

namespace Game.Battle.Map
{
    /**
     * Immutable tile on the BattleBoard
     */
    public class BattleBoardTile
    {
        public int x { get; }
        public int y { get; }
        public Vector2 coord { get; }
        public HashSet<Vector2> neighbours;
        public int cost = 1;
        public UnitObject unitOnTile;
        public bool walkable = true;
        public bool ContainsUnit => unitOnTile != null;

        public BattleBoardTile(int _x, int _y, HashSet<Vector2> _neighbours)
        {
            x = _x;
            y = _y;
            coord = new Vector2(x, y);
            neighbours = new HashSet<Vector2>(_neighbours);
        }
    }
    
    /**
     * Mutable grid of tiles representing BattleBoard of a battle/level/gameplay
     */
    public class BattleBoard
    {
        private Grid<BattleBoardTile> _board;
        public int size { get; }

        public BattleBoardTile GetTile(Vector2 v) => GetTile((int) v.x, (int) v.y);

        public BattleBoardTile GetTile(int x, int y) => _board[x, y];

        public UnitObject GetUnit(Vector2 v) => GetTile(v).unitOnTile;
        public UnitObject GetUnit(int x, int y) => GetTile(x, y).unitOnTile;
        public bool AnyUnitAt(Vector2 v) => GetTile(v).ContainsUnit;
        public bool AnyUnitAt(int x, int y) => GetTile(x, y).ContainsUnit;
        public bool ContainsCoord(Vector2 v) => ContainsCoord((int) v.x, (int) v.y);
        public bool ContainsCoord(int x, int y) => (x >= 0 && x < size - 1 && y >= 0 && y < size - 1);

        public BattleBoard(BattleSO _battleSO)
        {
            HashSet<Vector2> neighbours = new HashSet<Vector2>();
            _board = new Grid<BattleBoardTile>(_battleSO.mapSize, _battleSO.mapSize, 1.0f, Vector3.zero,
                (_, x, y) =>
                {
                    if (x - 1 >= 0) neighbours.Add(new Vector2(x - 1, y));
                    if (y - 1 >= 0) neighbours.Add(new Vector2(x, y - 1));
                    if (x + 1 < _battleSO.mapSize) neighbours.Add(new Vector2(x + 1, y));
                    if (y + 1 < _battleSO.mapSize) neighbours.Add(new Vector2(x, y + 1));
                    return new BattleBoardTile(x, y, neighbours);
                });
            neighbours.Clear();
            size = _battleSO.mapSize;
        }

        public void PlaceUnit(Vector2 v, UnitObject unit) => PlaceUnit((int) v.x, (int) v.y, unit);
        public void PlaceUnit(int x, int y, UnitObject unit)
        {
            BattleBoardTile tile = _board[x, y];
            if (AnyUnitAt(x, y))
            {
                Debug.LogError($"Unit already exists at Position {x}, {y}!");
                return;
            }
            
            if (!tile.walkable)
            {
                Debug.LogError($"Position {x}, {y} is not walkable!");
                return;
            }

            tile.unitOnTile = unit;
        }

        public void MoveUnitFromTo(BattleBoardTile from, BattleBoardTile to) => MoveUnitFromTo(from.coord, to.coord);
        public void MoveUnitFromTo(Vector2 from, Vector2 to)
        {
            BattleBoardTile fromTile = _board[from];
            BattleBoardTile toTile = _board[to];

            if (!AnyUnitAt(from))
            {
                Debug.LogError($"There is no Unit placed at {from}!");
                return;
            }
            
            if (AnyUnitAt(to))
            {
                Debug.LogError($"Unit already exists at Position {to}!");
                return;
            }
            
            if (!toTile.walkable)
            {
                Debug.LogError($"Position {to} is not walkable!");
                return;
            }

            toTile.unitOnTile = fromTile.unitOnTile;
            fromTile.unitOnTile = null;
        }
    }
}
