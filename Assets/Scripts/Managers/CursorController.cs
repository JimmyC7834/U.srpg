using System;
using UnityEngine;

namespace Game.Battle
{
    /**
     * Controller for cursor on the battle grid map
     */
    public class CursorController : MonoBehaviour
    {
        [SerializeField] private InputReader _input;

        // cursor movement
        [SerializeField] private float _cameraAngle;
        [SerializeField] private float _cursorMoveSpeed;
        [SerializeField] private Vector2 _velocity;
        [SerializeField] private Vector2 _mapCoord;
        [SerializeField] private Vector2 _prevMapCoord;
        private Vector2 _newMapCoord => Vector2Int.FloorToInt(transform.position.GameV3ToV2());
        public Vector2 MapCoord => _mapCoord;
        
        // ring sprite placement
        [SerializeField] private Transform _raycastPoint;
        [SerializeField] private Transform _markerSprite;

        public event Action<CursorController> OnMove = delegate {  };
        public event Action<CursorController> OnTileChange = delegate {  };
        public event Action<CursorController> OnConfirm = delegate {  };
        public event Action<CursorController> OnCancel = delegate {  };

        private void OnEnable()
        {
            _input.cursorMoveEvent += InputOnCursorMoveEvent;
            _input.cursorConfirmEvent += () => OnConfirm.Invoke(this);
            _input.cursorCancelEvent += () => OnCancel.Invoke(this);
        }

        private void Update()
        {
            UpdateCursorPosition();
        }

        private void UpdateCursorPosition()
        {
            if (_velocity.magnitude != 0)
            {
                transform.position += _cursorMoveSpeed * Time.deltaTime * _velocity.GameV2ToV3();
                _mapCoord = _newMapCoord;
                OnMove.Invoke(this);
                if (_mapCoord == _prevMapCoord) return;
                UpdateCoord();
            }
            else
            {
                // move toward center of the cell
                transform.position = Vector3.MoveTowards(
                    transform.position, 
                    _prevMapCoord.GameV2ToV3() + Vector3.one * .5f, 
                    _cursorMoveSpeed/2 * Time.deltaTime);
            }
        }

        private void UpdateCoord()
        {
            RaycastHit hit;
            Ray ray = new Ray(_raycastPoint.position, Vector3.down);
            if (Physics.Raycast(ray, out hit, 10f))
            {
                // ray hits, update cursor position
                _prevMapCoord = _mapCoord;
                _markerSprite.position = _prevMapCoord.GameV2ToV3() + hit.point.ExtractY();
                OnTileChange.Invoke(this);
                return;
            }
            
            // if ray no hit, outside of map -> reset v 
            _velocity = Vector2.zero;
            UpdateCursorPosition();
        }

        private void InputOnCursorMoveEvent(Vector2 v)
        {
            _velocity = v.GameRotateV2(Mathf.Deg2Rad * _cameraAngle);
        }
        
        public void MoveTo(Vector2 v)
        {
            transform.position = v.GameV2ToV3();
            _mapCoord = _newMapCoord;
            UpdateCoord();
            OnMove.Invoke(this);
            OnTileChange.Invoke(this);
        }
    }    
}