using UnityEngine;

namespace Game.Battle.Map
{
    public class TileHighlight : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        public void SetColor(Color _color) => _spriteRenderer.color = _color;
    }
}