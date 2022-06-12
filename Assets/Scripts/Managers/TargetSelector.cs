using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  Game.Battle
{
    public class TargetSelector : MonoBehaviour
    {
        [SerializeField] private BattleData _battleData;

        public struct RangeTile
        {
            public int x, y;
            public Vector2 parentCoord;
        }
        
        // public 
    }
}