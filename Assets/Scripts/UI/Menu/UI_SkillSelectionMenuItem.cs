using System;
using System.Collections;
using System.Collections.Generic;
using Game.UI;
using Game.Unit.Skill;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class UI_SkillSelectionMenuItem : MonoBehaviour, IUI_MenuItem<UI_SkillSelectionMenuItem>
    {
        public Image iconImg;
        public TMP_Text itemText;
        public SkillSO skill;
        public event Action<UI_SkillSelectionMenuItem> confirmEvent;

        public void Initialize(SkillSO _skill)
        {
            skill = _skill;
            iconImg.sprite = skill.icon;
            itemText.SetText(skill.name);
        }

        public void Confirm() => confirmEvent?.Invoke(this);
    }
}