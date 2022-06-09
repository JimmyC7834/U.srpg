using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
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
            public DataSet.UnitId unitId;
        }

        public UnitSpawnInfo[] unitSpawnInfo;
    }
}