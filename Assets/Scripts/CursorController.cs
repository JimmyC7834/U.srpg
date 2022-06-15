using System;
using System.Collections;
using System.Collections.Generic;
using Game.Battle.Map;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Battle
{
    public class CursorController : MonoBehaviour
    {
        [SerializeField] private InputReader _input;
        [SerializeField] private BattleData _battleData;

        // cursor movement
        [SerializeField] private float _cameraAngle;
        [SerializeField] private float _cursorMoveSpeed;
        [SerializeField] private Vector2 _velocity;
        [SerializeField] private Vector2 _mapCoord;
        [SerializeField] private Vector2 _prevMapCoord;
        private Vector2 _newMapCoord => Vector2Int.FloorToInt(transform.position.GameV3ToV2());
        public Vector2 MapCoord => _mapCoord;
        public BattleBoardTile CurrentTile => _battleData.battleBoard.GetTile(_mapCoord);
        
        // ring sprite placement
        [SerializeField] private Transform _raycastPoint;
        [SerializeField] private Transform _markerSprite;

        public Action<CursorController> OnMove;
        public Action<CursorController> OnTileChange;
        public Action<CursorController> OnConfirm;
        public Action<CursorController> OnCancel;

        private void OnEnable()
        {
            _input.cursorMoveEvent += InputOnCursorMoveEvent;
            _input.cursorConfirmEvent += () => OnConfirm.Invoke(this);
            _input.cursorCancelEvent += () => OnCancel.Invoke(this);
            
            // ignore ? check
            OnMove += (_) => { };
            OnTileChange += (_) => { };
            OnConfirm += (_) => { };
            OnCancel += (_) => { };

            OnTileChange += UpdateCurrentUnit;
        }

        private void Update()
        {
            UpdateCursorPosition();
        }

        private void UpdateCursorPosition()
        {
            if (_velocity.magnitude != 0)
            {
                transform.position += _velocity.GameV2ToV3() * _cursorMoveSpeed * Time.deltaTime;
                _mapCoord = _newMapCoord;
                OnMove.Invoke(this);
                if (_mapCoord != _prevMapCoord)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(new Ray(_raycastPoint.position, Vector3.down), out hit, 10f))
                    {
                        // ray hits, update cursor position
                        _prevMapCoord = _mapCoord;
                        _markerSprite.position = _prevMapCoord.GameV2ToV3() + hit.point.ExtractY();
                        OnTileChange.Invoke(this);
                    }
                    else
                    {
                        // if ray no hit, outside of map -> reset v 
                        _velocity = Vector2.zero;
                        UpdateCursorPosition();
                    }
                }
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

        private void InputOnCursorMoveEvent(Vector2 v)
        {
            _velocity = v.GameRotateV2(Mathf.Deg2Rad * _cameraAngle);
        }

        private void UpdateCurrentUnit(CursorController _)
        {
            _battleData.SetCurrentUnit(_battleData.battleBoard.GetUnit(transform.position.GameV3ToV2()));
        }
    }    
}