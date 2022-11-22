using Game.Battle.Map;
using Game.DataSet;
using UnityEditor;
using UnityEngine;

namespace Game
{
    public class BoardEditor : MonoBehaviour
    {
        [SerializeField] private TerrainDataSetSO _terrainData;
        [SerializeField] private InputReader _input;
        
        [SerializeField] private Vector2Int _size;
        [SerializeField] private TerrainID _defaultTerrian;
        [SerializeField] private TerrainID _selectedTerrian;
        [SerializeField] private float _debugDrawHeight;

        [SerializeField] private BattleBoardSO _boardData;

        [SerializeField] private Vector2 _mousePos;
        private Grid<TerrainID> _grid;

        private void Awake()
        {
            _grid = new Grid<TerrainID>(_size.x, _size.y, 1f, Vector3.zero, (_, _, _) => _defaultTerrian);
            DebugDrawGrid();
            // _input.placeTerrainEvent += PlaceTerrin;
            // _input.boardEditorMouseMoveEvent += UpdateMousePosition;
            
            // _input.EnableBoardEditorInput();
        }

        private void UpdateMousePosition(Vector2 v2)
        {
            _mousePos = v2;
        }
        
        private void PlaceTerrin()
        {
            // Ray castPoint = Camera.main.ScreenPointToRay(_mousePos);
            // RaycastHit hit = new RaycastHit();
            //
            // if (Physics.Raycast(castPoint, out hit, Mathf.Infinity))
            // {
            //     _grid.SetValue((int)hit.point.x, (int)hit.point.z, _selectedTerrian);
            //     Debug.Log(hit.point);
            // }
        }

        [ContextMenu("Save")]
        private void SaveData()
        {
            _boardData.size = _size;
            _boardData.terrainIds = new TerrainID[_size.x, _size.y];
            for (int x = 0; x < _size.x; x++)
            {
                for (int y = 0; y < _size.y; y++)
                {
                    _boardData.terrainIds[x, y] = _grid.GetValue(x, y);
                }
            }
        }
        
        [ContextMenu("Load")]
        private void LoadData()
        {
            _size = _boardData.size;
            _grid = new Grid<TerrainID>(_size.x, _size.y, 1f, Vector3.zero, (_, x, y) => _boardData.terrainIds[x, y]);
            DebugDrawGrid();
        }
        
        private void DebugDrawGrid()
        {
            for (int x = 0; x < _size.x; x++)
            {
                for (int y = 0; y < _size.y; y++)
                {
                    Debug.DrawLine(new Vector3(x, _debugDrawHeight, y), new Vector3(x + 1, _debugDrawHeight, y), Color.cyan, 1000f);
                    Debug.DrawLine(new Vector3(x, _debugDrawHeight, y), new Vector3(x, _debugDrawHeight, y + 1), Color.cyan, 100f);
                }
            }
            
            Debug.DrawLine(new Vector3(_size.x, _debugDrawHeight, 0), new Vector3(_size.x, _debugDrawHeight, _size.y), Color.cyan, 1000f);
            Debug.DrawLine(new Vector3(0, _debugDrawHeight, _size.y), new Vector3(_size.x, _debugDrawHeight, _size.y), Color.cyan, 1000f);
        }

        private void OnDrawGizmosSelected()
        {
            if (_grid == null) return;
            GUIStyle style = new GUIStyle();

            for (int x = 0; x < _size.x; x++)
            {
                for (int y = 0; y < _size.y; y++)
                {
                    TerrainSO terrain = _terrainData[_grid[x, y]];
                    style.normal.textColor = terrain.debugColor;
                    Handles.Label(new Vector3(x + .5f, _debugDrawHeight + .5f, y + .5f), $"{terrain.cost}", style);
                }
            }
        }
    }
}