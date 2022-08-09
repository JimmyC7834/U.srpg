using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Unit;
using UnityEngine;
using UnityEngine.Pool;

namespace Game.Battle
{
    public class UnitManager : MonoBehaviour
    {
        [SerializeField] private BattleService _battleService;
        
        private IObjectPool<UnitObject> _pool;
        private List<UnitObject> _units;
        public List<UnitObject> currentKokuUnits { get => GetCurrentKokuUnits(_battleService.currentKoku); }

        [SerializeField] private DataSet.UnitDatasetSO _unitDataset;
        [SerializeField] private UnitTimelineIconController _timelineIconController;
        [SerializeField] private UnitObject _prefab;

        public void Initialize(BattleSO.UnitSpawnInfo[] unitSpawnInfo)
        {
            _pool = new ObjectPool<UnitObject>(
                CreateNewUnit,
                PoolUnit,
                ReleaseUnit
                );

            _units = new List<UnitObject>();

            for (int i = 0; i < unitSpawnInfo.Length; i++)
            {
                SpawnUnitAt(unitSpawnInfo[i].unitId, unitSpawnInfo[i].coord);
            }

            // _battleService.battleTurnManager.OnKokuChanged += UpdateCurrentKokuUnits;
        }

        private List<UnitObject> GetCurrentKokuUnits(int koku)
        {
            return _units.Where(unit => unit.param.MP == koku).ToList();
        }
        
        private UnitObject CreateNewUnit()
        {
            return Instantiate(_prefab);
        }

        private void PoolUnit(UnitObject unit)
        {
            unit.gameObject.SetActive(true);
        }
        
        private void ReleaseUnit(UnitObject unit)
        {
            unit.gameObject.SetActive(false);
        }
        
        public void SpawnUnitAt(UnitId id, Vector2Int coord)
        {
            UnitObject newUnit = _pool.Get();
            newUnit.InitializeWith(_unitDataset[id], _battleService);
            _timelineIconController.RegisterIconOn(newUnit);
            _units.Add(newUnit);
            PlaceUnitObjectAt(newUnit, coord);
            _battleService.battleBoard.PlaceUnit(coord, newUnit);
        }

        private void PlaceUnitObjectAt(UnitObject unit, Vector2Int coord)
        {
            RaycastHit hit;
            if (Physics.Raycast(new Ray(new Vector3(coord.x, 10, coord.y), Vector3.down), out hit, 20f))
            {
                // ray hits, update unit position
                unit.transform.position = hit.point;
            }
            else
            {
                Debug.LogError($"Failed to place unit {unit} at {coord}.");
            }
        }
    }
}
