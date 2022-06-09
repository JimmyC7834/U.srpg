using System;
using System.Collections;
using System.Collections.Generic;
using Game.UI;
using Game.Unit;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private EventSystem _eventSystem;

        [SerializeField] private UI_SkillSelectionMenu _skillList;

        public void OpenSkillSelectionMenu(UnitObject unit, Action<UI_SkillSelectionMenuItem> callback)
        {
            _skillList.OpenMenu(unit.partTree.GetAllSkills(), (item) => callback.Invoke(item));
            _eventSystem.SetSelectedGameObject(_skillList.GetItemAt(0).gameObject);
            callback += (_) => _skillList.CloseMenu();
        }
    }
}