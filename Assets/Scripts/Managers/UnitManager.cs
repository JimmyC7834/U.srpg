using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Unit;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Pool;
using Utils;
using Object = System.Object;

namespace Game.Battle
{
    /**
     * Manager of units of a single Battle
     */
    public class UnitManager : MonoBehaviour
    {
        [SerializeField] private BattleService _battleService;
        
        private GameObjectPool<UnitObject> _pool;
        private PriorityQueue<UnitObject, int> _heap;
        private List<UnitObject> _currentKokuUnits;

        [SerializeField] private DataSet.UnitDatasetSO _unitDataset;
        [SerializeField] private UnitObject _prefab;

        public event Action<UnitObject> OnUnitSpawned = delegate {  };
        // public event Action<UnitObject, Vector2> OnUnitMoved = delegate {  };

        public void Initialize(BattleSO.UnitSpawnInfo[] unitSpawnInfo)
        {
            _pool = new GameObjectPool<UnitObject>(_prefab, transform);

            _currentKokuUnits = new List<UnitObject>();
            _heap = new PriorityQueue<UnitObject, int>(new UnitApComparer());

            for (int i = 0; i < unitSpawnInfo.Length; i++)
            {
                SpawnUnitAt(unitSpawnInfo[i].unitId, unitSpawnInfo[i].coord);
            }

            _battleService.battleTurnManager.OnKokuChanged += UpdateCurrentKokuUnits;
        }

        private class UnitApComparer : IComparer<int>
        {
            public int Compare(int a, int other)
            {
                return other - a;
            }
        }

        private void UpdateCurrentKokuUnits(int koku)
        {
            while (_heap.Count != 0 && _heap.Peek().stats.AP == koku)
                _currentKokuUnits.Add(_heap.Dequeue());
        }

        public List<UnitObject> GetCurrentUnits()
        {
            return new List<UnitObject>(_currentKokuUnits);
        }
        
        public void ReturnToHeap(UnitObject unit)
        {
            if (!_currentKokuUnits.Contains(unit)) return;
            _currentKokuUnits.Remove(unit);
            _heap.Enqueue(unit, unit.stats.AP);
        }
        
        public bool NoCurrentUnits() => _currentKokuUnits.Count == 0;

        public void SpawnUnitAt(UnitId id, Vector2Int coord)
        {
            UnitObject newUnit = _pool.Get(
                obj => obj.InitializeWith(_unitDataset[id], _battleService));
            
            _heap.Enqueue(newUnit, newUnit.stats.AP);
            PlaceUnitObjectAt(newUnit, coord);
            
            OnUnitSpawned.Invoke(newUnit);
            _battleService.battleBoard.PlaceUnit(coord, newUnit);
        }

        private void PlaceUnitObjectAt(UnitObject unit, Vector2Int coord)
        {
            // just to get rid of possible uninitialized error
            RaycastHit hit = new RaycastHit();
            Ray ray = new Ray(new Vector3(coord.x, 10, coord.y), Vector3.down);
            Assert.IsTrue(Physics.Raycast(ray, out hit, 20f), 
                $"Failed to place unit {unit} at {coord}.");
            unit.transform.position = hit.point;
        }
    }
}
