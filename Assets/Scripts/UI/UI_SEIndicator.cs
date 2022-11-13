using System.Collections;
using System.Collections.Generic;
using Game.Unit;
using Game.Unit.StatusEffect;
using UnityEngine;
using TMPro;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

namespace Game.UI
{
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
            // _icon.sprite = register.statusEffect.icon;
            // UpdateCountNumber(register);
        }

        public void UpdateCountNumber()
        {
            count = _statusEffect.count;
            _label.SetText(count.ToString());
        }
    }
}