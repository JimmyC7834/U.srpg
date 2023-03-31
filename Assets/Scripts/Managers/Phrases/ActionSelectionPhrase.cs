namespace Game.Battle
{
    public class ActionSelectionPhrase : Phrase
    {
        public ActionSelectionPhrase(BattlePhraseManager parent) : base(parent) { }

        public override void Enter()
        {
            _input.DisableAllInput();
            _input.EnableMenuNaviInput();
            _input.menuLeftEvent += ActionEnd;
            _input.menuUpEvent += ActionEnd;
            _input.menuConfirmEvent += OnConfirm;
            _input.menuCancelEvent += OnCancel;
            
            _cursor.gameObject.SetActive(false);
            battleService.battleUIManager.ToggleActionMenu(true);
        }
        
        public override void Exit()
        {
            _input.menuUpEvent -= ActionEnd;
            _input.menuLeftEvent -= ActionEnd;
            _input.menuCancelEvent -= OnCancel;
            _input.menuConfirmEvent -= OnConfirm;
            _cursor.gameObject.SetActive(true);
            battleService.battleUIManager.ToggleActionMenu(false);
        }
        
        private void ActionEnd()
        {
            battleService.unitManager.ReturnToHeap(battleService.CurrentUnit);
            battleService.CurrentUnit.Data.EndAction();
            // battleService.uiManager.CloseSkillSelectionMenu();
            _parent.Pop();
            if (battleService.unitManager.NoCurrentUnits())
            {
                _parent.Push(new HandleKokuPhrase(_parent));
                return;
            }
            
            _parent.Push(new UnitSelectionPhrase(_parent));
        }

        private void OnCancel()
        {
            // back to unit selection pharse
            _parent.Pop();
            _parent.Push(new UnitSelectionPhrase(_parent));
        }
        
        private void OnConfirm()
        {
            // back to unit selection pharse
            _parent.Pop();
            _parent.Push(new SkillSelectionPhrase(_parent));
        }
    }
}