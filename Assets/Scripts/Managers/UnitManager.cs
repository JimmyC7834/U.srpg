using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Unit;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Pool;

namespace Game.Battle
{
    public class UnitManager : MonoBehaviour
    {
        [SerializeField] private BattleService _battleService;
        
        private GameObjectPool<UnitObject> _pool;
        private List<UnitObject> _units;
        public List<UnitObject> currentKokuUnits;

        [SerializeField] private DataSet.UnitDatasetSO _unitDataset;
        [SerializeField] private UnitObject _prefab;

        public event Action<UnitObject> OnUnitSpawned = delegate {  };
        
        public void Initialize(BattleSO.UnitSpawnInfo[] unitSpawnInfo)
        {
            _pool = new GameObjectPool<UnitObject>(_prefab, transform);

            currentKokuUnits = new List<UnitObject>();
            _units = new List<UnitObject>();

            for (int i = 0; i < unitSpawnInfo.Length; i++)
            {
                SpawnUnitAt(unitSpawnInfo[i].unitId, unitSpawnInfo[i].coord);
            }

            _battleService.battleTurnManager.OnKokuChanged += UpdateCurrentKokuUnits;
        }

        public void UpdateCurrentKokuUnits(int koku)
        {
            currentKokuUnits = _units.Where(unit => unit.param.AP == koku).ToList();
        }

        public void SpawnUnitAt(UnitId id, Vector2Int coord)
        {
            UnitObject newUnit = _pool.Get(
                obj => obj.InitializeWith(_unitDataset[id], _battleService));
            
            _units.Add(newUnit);
            PlaceUnitObjectAt(newUnit, coord);
            
            OnUnitSpawned.Invoke(newUnit);
            _battleService.battleBoard.PlaceUnit(coord, newUnit);
        }

        private void PlaceUnitObjectAt(UnitObject unit, Vector2Int coord)
        {
            RaycastHit hit;
            Ray ray = new Ray(new Vector3(coord.x, 10, coord.y), Vector3.down);
            Assert.IsTrue(Physics.Raycast(ray, out hit, 20f), 
                $"Failed to place unit {unit} at {coord}.");
            unit.transform.position = hit.point;
        }
    }
}
