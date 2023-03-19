using UnityEngine;

namespace Game.UI
{
    public class UI_DamageIndicator : UI_PopUpText
    {
        [SerializeField] private float _floatSpeed;
        private new void Initialize(Vector3 worldPosition, Vector2 popUpDirection, float speed, string text) { }

        public void Initialize(Vector3 worldPosition, int value)
        {
            Vector2 v2 = new Vector2(
                Mathf.Cos(Random.Range(0, 2f * Mathf.PI)),
                Mathf.Sin(Random.Range(0, 2f * Mathf.PI))
                );
            base.Initialize(worldPosition, v2, _floatSpeed, value.ToString());
        }
    }
}