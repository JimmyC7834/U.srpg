using System.Collections;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class UI_PopUpText : MonoBehaviour
    {
        [SerializeField] private TMP_Text _textMesh;
        [SerializeField] private Transform _transform;
        [SerializeField] private float _lifespan;

        private float _speed;
        private Vector3 _popUpDirection;
        
       public virtual void Initialize(Vector3 worldPosition, Vector2 popUpDirection, float speed, string text)
        {
            _textMesh.SetText(text);
            _transform.position = worldPosition;
            _popUpDirection = popUpDirection.normalized.GameV2ToV3();
            _speed = speed;

            StartCoroutine(PopUp());
            gameObject.SetActive(true);
        }

        public IEnumerator PopUp()
        {
            while (_lifespan > 0)
            {
                _transform.position += _speed * Time.deltaTime * _popUpDirection;
                _lifespan -= Time.deltaTime;
                yield return null;
            }
            
            gameObject.SetActive(false);
        }
    }
}