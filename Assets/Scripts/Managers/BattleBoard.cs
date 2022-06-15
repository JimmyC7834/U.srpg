using System.Collections;
using System.Collections.Generic;
using Game.Unit;
using UnityEngine;

namespace Game.Battle.Map
{
    public class BattleBoardTile
    {
        public int x { get; }
        public int y { get; }
        public Vector2 coord { get; }
        public HashSet<Vector2> neighbours;
        public int cost = 1;
        public UnitObject unitOnTile;
        public bool walkable = true;

        public BattleBoardTile(int _x, int _y)
        {
            x = _x;
            y = _y;
            coord = new Vector2(x, y);
            neighbours = new HashSet<Vector2>();
        }
    }
    
    public class BattleBoard
    {
        private int _size;
        private Grid<BattleBoardTile> _board;
        
        public BattleBoardTile GetTile(Vector2 v) => GetTile((int) v.x, (int) v.y);

        public BattleBoardTile GetTile(int x, int y) => _board[x, y];
        // public BattleBoardTile GetTile(int x, int y)
        // {
        //     if (x < 0 || x > _size - 1 || y < 0 || y > _size - 1)
        //     {
        //         Debug.LogError($"Coord out of Board! [{x}, {y}]");
        //         return _board[0, 0];
        //     }
        //
        //     return _board[x, y];
        // }
        
        public UnitObject GetUnit(Vector2 v) => GetTile(v).unitOnTile;
        public UnitObject GetUnit(int x, int y) => GetTile(x, y).unitOnTile;
        public bool AnyUnitAt(Vector2 v) => GetUnit(v) != null;
        public bool AnyUnitAt(int x, int y) => GetUnit(x, y) != null;
        public bool CoordOnBoard(Vector2 v) => CoordOnBoard((int) v.x, (int) v.y);
        public bool CoordOnBoard(int x, int y) => (x > 0 && x < _size - 1 && y > 0 && y < _size - 1);

        public BattleBoard(BattleSO _battleSO)
        {
            _board = new Grid<BattleBoardTile>(_battleSO.mapSize, _battleSO.mapSize, 1.0f, Vector3.zero,
                (x, y) =>
                {
                    BattleBoardTile newTile = new BattleBoardTile(x, y);
                    if (x - 1 >= 0)
                        newTile.neighbours.Add(new Vector2(x - 1, y));
                    if (x + 1 < _battleSO.mapSize)
                        newTile.neighbours.Add(new Vector2(x + 1, y));
                    if (y - 1 >= 0)
                        newTile.neighbours.Add(new Vector2(x, y - 1));
                    if (y + 1 < _battleSO.mapSize)
                        newTile.neighbours.Add(new Vector2(x, y + 1));
                    return newTile;
                });
            _size = _battleSO.mapSize;
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

        public void MoveUnitFromTo(Vector2Int from, Vector2Int to)
        {
            BattleBoardTile fromTile = _board[from];
            BattleBoardTile toTile = _board[to];

            if (AnyUnitAt(from))
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
