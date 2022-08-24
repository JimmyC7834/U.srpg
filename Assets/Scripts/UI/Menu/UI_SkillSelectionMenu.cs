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
        private event Action<UI_SkillSelectionMenuItem> confirmEvent;
        
        public void OpenMenu(List<SkillSO> skills, Action<UI_SkillSelectionMenuItem> callback)
        {
            gameObject.SetActive(true);

            foreach (SkillSO skill in skills)
            {
                UI_SkillSelectionMenuItem item = AddItem(skill, (item) =>
                {
                    confirmEvent?.Invoke(item);
                });
            }

            confirmEvent += callback;
        }

        public UI_SkillSelectionMenuItem AddItem(SkillSO skill, Action<UI_SkillSelectionMenuItem> callback = null)
        {
            UI_SkillSelectionMenuItem newItem = base.AddItem(callback);
            newItem.Initialize(skill);
            return newItem;
        }
        
        public void CloseMenu()
        {
            Clear();
            gameObject.SetActive(false);
            confirmEvent = null;
        }
    }
}