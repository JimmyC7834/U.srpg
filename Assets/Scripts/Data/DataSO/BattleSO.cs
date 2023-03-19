using System;
using Game.Unit;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "Game/Battle/SO")]
    public class BattleSO : ScriptableObject
    {
        [Serializable]
        public struct UnitSpawnInfo
        {
            public Vector2Int coord;
            public UnitId unitId;
        }

        [SerializeField] private UnitSpawnInfo[] _unitSpawnInfo;
        [SerializeField] private int _mapSize;

        public UnitSpawnInfo[] unitSpawnInfos { get => _unitSpawnInfo; }
        public int mapSize { get => _mapSize; }
        
    }
}