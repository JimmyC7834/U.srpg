using Game.Unit.StatusEffect;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    // TODO: adapt this visual to different SEs
    public class UI_SEIndicator : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _label;
        [SerializeField] private int count;
        private StatusEffect _statusEffect;

        public void Initialize(StatusEffect statusEffect)
        {
            _statusEffect = statusEffect;
            UpdateCountNumber();
        }

        public void UpdateCountNumber()
        {
            count = 0;
            _label.SetText(count.ToString());
        }
    }
}