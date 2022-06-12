using System;
using System.Collections;
using System.Collections.Generic;
using Game.Unit;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

namespace Game.Battle.Map
{
    public enum TileHighlightColor
    {
        InTargetRange,
        InAttackRange,
        Count
    }
    
    public class MapHighlighter : MonoBehaviour
    {
        [SerializeField] private Color[] _colors;
        [SerializeField] private GameObject _prefab;

        private void Awake()
        {
            if (_colors.Length != (int) TileHighlightColor.Count)
                Debug.LogError($"Wrong Number of Tile Highlight Colors!");
        }

        public void HighlightTiles(List<Vector2> poss, TileHighlightColor color)
        {
            foreach (Vector2 pos in poss)
            {
                // Debug.Log(pos);
                TileHighlight tileHighlight = Instantiate(_prefab, transform).GetComponent<TileHighlight>();
                tileHighlight.SetColor(_colors[(int) color]);
                RaycastHit hit;
                if (Physics.Raycast(new Ray(pos.GameV2ToV3() + Vector3.up * 10, Vector3.down), out hit, 10f))
                {
                    // ray hits, update cursor position
                    tileHighlight.transform.position = pos.GameV2ToV3() + hit.point.ExtractY() + Vector3.up * .1f;
                }
            }
        }
    }
}