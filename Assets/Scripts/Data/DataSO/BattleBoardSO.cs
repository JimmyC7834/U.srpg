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