using System.Collections;
using System.Collections.Generic;
using Game.DataSet;
using UnityEngine;

namespace Game.Battle.Map
{
    [CreateAssetMenu(menuName = "Game/DataEntry/Terrain")]
    public class TerrainSO : DataEntrySO<TerrainID>
    {
        public int cost;
        public bool walkable;
        public Color debugColor;
    }
    
    public enum TerrainID 
    {
        GlassField = 10,
        DirtField = 20,
    }
}