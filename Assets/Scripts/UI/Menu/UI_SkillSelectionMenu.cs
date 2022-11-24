using System;
using System.Collections.Generic;
using Game.Unit.Skill;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.UI
{
    public class UI_SkillSelectionMenu : UI_View
    {
        [SerializeField] private InputReader _input;
        [SerializeField] private UI_DataEntryMenu<SkillSO, SkillId> _skillMenu;
        private event Action<SkillSO> onConfirm = delegate { };
        private event Action onCancel = delegate { };
        
        protected override void Enter()
        {
            _input.EnableMenuNaviInput();
            _input.menuCancelEvent += CancelSkillSelection;
        }

        protected override void Exit()
        {
            _input.DisableAllInput();
            _input.menuCancelEvent -= CancelSkillSelection;
        }

        public void OpenMenu(List<SkillSO> skills, Action<SkillSO> _onConfirm, Action _onCancel)
        {
            onConfirm = _onConfirm;
            onCancel = _onCancel;
            _skillMenu.OpenMenu(skills, onConfirm);
            EventSystem.current.SetSelectedGameObject(_skillMenu.GetItemObject(0).gameObject);
        }

        public void CancelSkillSelection()
        {
            _skillMenu.CloseMenu();
            onCancel.Invoke();
        }
    }
}