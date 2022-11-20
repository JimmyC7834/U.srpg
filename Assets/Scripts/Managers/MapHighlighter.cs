using System;
using System.Collections;
using System.Collections.Generic;
using Game.Unit;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Pool;

namespace Game.Battle.Map
{
    public enum TileHighlightColor
    {
        InTargetRange,
        InAttackRange,
        Count
    }
    
    /**
     * Provides interface to highlight tile on the map
     */
    public class MapHighlighter : MonoBehaviour
    {
        // TODO: Need to rewrite
        [SerializeField] private Color[] _colors;
        [SerializeField] private TileHighlight _prefab;
        private GameObjectPool<TileHighlight> _pool;
        private List<TileHighlight> _highlights;

        private void Awake()
        {
            if (_colors.Length != (int) TileHighlightColor.Count)
                Debug.LogError($"Wrong Number of Tile Highlight Colors!");
            _pool = new GameObjectPool<TileHighlight>(_prefab, transform);
            _highlights = new List<TileHighlight>();
        }

        public void HighlightTiles(IEnumerable<Vector2> poss, TileHighlightColor color)
        {
            foreach (Vector2 pos in poss)
            {
                // Debug.Log(pos);
                TileHighlight tileHighlight = _pool.Get(obj =>
                {
                    _highlights.Add(obj);
                    obj.SetColor(_colors[(int) color]);
                });
                RaycastHit hit;
                Ray ray = new Ray(pos.GameV2ToV3() + Vector3.up * 10, Vector3.down);
                if (Physics.Raycast(ray, out hit, 10f))
                {
                    // ray hits, update cursor position
                    tileHighlight.transform.position = pos.GameV2ToV3() + hit.point.ExtractY() + Vector3.up * .01f;
                }
            }
        }

        public void RemoveHighlights()
        {
            foreach (TileHighlight highlight in _highlights)
            {
                _pool.Release(highlight);
            }
            
            _highlights.Clear();
        }
    }
}