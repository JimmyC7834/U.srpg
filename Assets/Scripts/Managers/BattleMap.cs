using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Battle.Map
{
    public class BattleMapTile
    {
        public int x;
        public int y;
        public HashSet<Vector2> neighbours;
        public int cost = 1;

        public BattleMapTile(int _x, int _y)
        {
            x = _x;
            y = _y;
            neighbours = new HashSet<Vector2>();
        }
    }
    
    public class BattleMap : MonoBehaviour
    {
        private Grid<BattleMapTile> _map;

        public BattleMapTile GetValue(Vector2 v) => _map.GetValue((int) v.x, (int) v.y);
        public BattleMapTile GetValue(int x, int y) => _map.GetValue(x, y);
        
        public void Initialize(BattleSO _battleSO)
        {
            _map = new Grid<BattleMapTile>(_battleSO.mapSize, _battleSO.mapSize, 1.0f, Vector3.zero,
                    (x, y) =>
                    {
                        BattleMapTile newTile = new BattleMapTile(x, y);
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
        }
    }
}