using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Game.UI
{
    public class UI_DamageIndicator : MonoBehaviour
    {
        [SerializeField] private TMP_Text _textMesh;
        [SerializeField] private Transform _transform;
        [SerializeField] private float _lifespan;

        [SerializeField] private float _terminalVelocity;
        [SerializeField] private Vector3 _velocity;
        [SerializeField] private float _gravity;
        [SerializeField] private float _initialVelocityMag;
        [SerializeField] private float _popUpAngle;

        public void Initialize(Vector3 worldPosition, int value)
        {
            _textMesh.SetText(value.ToString());
            float r = Random.Range(0, _popUpAngle);
            r = (r * 1.5f + (90 - r)) * Mathf.Deg2Rad;
            _transform.position = worldPosition;
            _velocity = new Vector2(
                _initialVelocityMag * Mathf.Cos(r),
                _initialVelocityMag * Mathf.Sin(r)
                );
            
            
            StartCoroutine(PopUp());
            gameObject.SetActive(true);
        }

        public IEnumerator PopUp()
        {
            while (_lifespan > 0)
            {
                _transform.position += _velocity * Time.deltaTime;
                if (_velocity.magnitude > _terminalVelocity)
                {
                    _velocity = _velocity.normalized * _terminalVelocity;
                }
                else
                {
                    _velocity += Vector3.down * _gravity * Time.deltaTime;
                }
                
                _lifespan -= Time.deltaTime;
                yield return null;
            }
            
            gameObject.SetActive(false);
        }
    }
}