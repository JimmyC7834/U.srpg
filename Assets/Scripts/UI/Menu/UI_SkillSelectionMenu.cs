using System;
using System.Collections.Generic;
using Game.Unit.Skill;
using UnityEngine.EventSystems;

namespace Game.UI
{
    public class UI_SkillSelectionMenu : UI_DataEntryMenu<SkillSO, SkillId>
    {
        protected override void Enter()
        {
            EventSystem.current.SetSelectedGameObject(items[0].gameObject);
        }

        public void OpenMenu(List<SkillSO> skills, Action<SkillSO> _callback)
        {
            Clear();
            gameObject.SetActive(true);
            callback += _callback;
            
            foreach (SkillSO skill in skills)
                AddItem(skill);
        }

        public override void OnConfirmed(SkillSO dataEntry)
        {
            CloseMenu();
        }

        public void CloseMenu()
        {
            Clear();
            gameObject.SetActive(false);
        }
    }
}