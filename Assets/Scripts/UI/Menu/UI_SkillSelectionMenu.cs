using System;
using System.Collections;
using System.Collections.Generic;
using Game.DataSet;
using Game.UI;
using Game.Unit.Skill;
using JetBrains.Annotations;
using UnityEngine;

namespace Game.UI
{
    public class UI_SkillSelectionMenu : UI_Menu<UI_SkillSelectionMenuItem>
    {
        [SerializeField] private SkillDataSetSO _skillDataSet;
        private event Action<UI_SkillSelectionMenuItem> confirmEvent;
        
        public void OpenMenu(List<SkillSO> skills, Action<UI_SkillSelectionMenuItem> callback)
        {
            gameObject.SetActive(true);

            foreach (SkillSO skill in skills)
            {
                // Debug.Log($"Adding Skill Menu Item, id: {skill.id}");
                UI_SkillSelectionMenuItem item = AddItem((item) =>
                {
                    confirmEvent?.Invoke(item);
                });
                // Debug.Log($"Added Skill Menu Item: {item}");
                item.Initialize(skill);
            }

            confirmEvent += callback;
        }

        public void CloseMenu()
        {
            Clear();
            gameObject.SetActive(false);
        }
    }
}