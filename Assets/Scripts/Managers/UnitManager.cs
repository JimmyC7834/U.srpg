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
        private IObjectPool<UnitObject> _pool;
        private Dictionary<Vector2Int, UnitObject> _unitCoordDict;

        [SerializeField] private DataSet.UnitDatasetSO _unitDataset;
        [SerializeField] private UnitObject _prefab;

        public void Initialize(BattleSO.UnitSpawnInfo[] unitSpawnInfo)
        {
            _pool = new ObjectPool<UnitObject>(
                CreateNewUnit,
                PoolUnit,
                ReleaseUnit
                );

            _unitCoordDict = new Dictionary<Vector2Int, UnitObject>();

            for (int i = 0; i < unitSpawnInfo.Length; i++)
            {
                SpawnUnitAt(unitSpawnInfo[i].unitId, unitSpawnInfo[i].coord);
            }
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
        
        public void SpawnUnitAt(DataSet.UnitId id, Vector2Int coord)
        {
            if (_unitCoordDict.ContainsKey(coord))
            {
                Debug.LogWarning($"Unit already exists at {coord}");
                return;
            }

            UnitObject newUnit = _pool.Get();
            newUnit.InitializeWith(_unitDataset[id]);
            PlaceUnitObjectAt(newUnit, coord);
            _unitCoordDict.Add(coord, newUnit);
        }

        private void PlaceUnitObjectAt(UnitObject unit, Vector2Int coord)
        {
            RaycastHit hit;
            if (Physics.Raycast(new Ray(new Vector3(coord.x, 10, coord.y), Vector3.down), out hit, 20f))
            {
                // ray hits, update cursor position
                unit.transform.position = hit.point;
            }
            else
            {
                Debug.LogError($"Failed to place unit {unit} at {coord}.");
            }
        }

        public UnitObject GetUnitAt(Vector2Int coord)
        {
            if (_unitCoordDict.ContainsKey(coord))
                return _unitCoordDict[coord];
            return null;
        }
    }
}
