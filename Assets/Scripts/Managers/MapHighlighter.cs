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
    
    public class MapHighlighter : MonoBehaviour
    {
        [SerializeField] private Color[] _colors;
        [SerializeField] private GameObject _prefab;
        private IObjectPool<TileHighlight> _pool;
        private List<TileHighlight> _highlights;

        private void Awake()
        {
            if (_colors.Length != (int) TileHighlightColor.Count)
                Debug.LogError($"Wrong Number of Tile Highlight Colors!");
            _pool = new ObjectPool<TileHighlight>(
                CreateNewTileHighlight,
                PoolTileHighlight,
                ReleaseTileHighlight
            );
            _highlights = new List<TileHighlight>();
        }

        private TileHighlight CreateNewTileHighlight()
        {
            return Instantiate(_prefab, transform).GetComponent<TileHighlight>();
        }
        
        private void PoolTileHighlight(TileHighlight tileHighlight)
        {
            tileHighlight.gameObject.SetActive(true);
            _highlights.Add(tileHighlight);
        }
        
        private void ReleaseTileHighlight(TileHighlight tileHighlight)
        {
            tileHighlight.gameObject.SetActive(false);
        }

        public void HighlightTiles(IEnumerable<Vector2> poss, TileHighlightColor color)
        {
            foreach (Vector2 pos in poss)
            {
                // Debug.Log(pos);
                TileHighlight tileHighlight = _pool.Get();
                tileHighlight.SetColor(_colors[(int) color]);
                RaycastHit hit;
                if (Physics.Raycast(new Ray(pos.GameV2ToV3() + Vector3.up * 10, Vector3.down), out hit, 10f))
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