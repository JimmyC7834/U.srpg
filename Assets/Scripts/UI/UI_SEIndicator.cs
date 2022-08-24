using System.Collections;
using System.Collections.Generic;
using Game.Unit;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Game.UI
{
    public class UI_SEIndicator : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _label;
        [SerializeField] private int count;

        public void Initialize(StatusEffectRegister register)
        {
            count = register.turnsLeft;
            _label.SetText(count.ToString());
            // _icon.sprite = register.statusEffect.icon;
            // UpdateCountNumber(register);
        }

        public void UpdateCountNumber(StatusEffectRegister register)
        {
            count = register.turnsLeft;
            _label.SetText(count.ToString());
        }
    }
}