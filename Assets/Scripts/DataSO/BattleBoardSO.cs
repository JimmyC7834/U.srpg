using System.Collections;
using System.Collections.Generic;
using Game.DataSet;
using UnityEngine;

namespace Game.Battle.Map
{
    [CreateAssetMenu(menuName = "Game/DataEntry/BattleBoard")]
    public class BattleBoardSO : ScriptableObject
    {
        public Vector2Int size;
        public TerrainID[,] terrainIds;
    }
}