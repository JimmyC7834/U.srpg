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
    public class UI_SkillSelectionMenu : UI_DataEntryMenu<SkillSO, SkillId>
    {
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
            gameObject.SetActive(false);
        }
    }
}