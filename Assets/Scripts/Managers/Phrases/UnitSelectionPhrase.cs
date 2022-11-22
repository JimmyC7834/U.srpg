using Game.Unit;
using UnityEngine;

namespace Game.Battle
{
    public class UnitSelectionPhrase : Phrase
    {
        public UnitSelectionPhrase(BattlePhraseManager parent) : base(parent) { }

        public override void Enter()
        {
            _cursor.OnConfirm += OnConfirm;
            _input.EnableMapNaviInput();

            if (battleService.unitManager.NoCurrentUnits())
            {
                _parent.Pop();
                _parent.Push(new HandleKokuPhrase(_parent));
            }
        }

        public override void Exit()
        {
            _cursor.OnConfirm -= OnConfirm;
        }

        private void OnConfirm(CursorController obj)
        {
            UnitObject unit = battleService.CurrentUnit;
            if (unit == null) return;
            if (unit._team != BattleTeam.Player) return;
            if (!battleService.CanSelectCurrentUnit) return;
            Debug.Log($"Confrimed on unit {battleService.CurrentTile}");
            _parent.Pop();
            _parent.Push(new ActionSelectionPhrase(_parent));
        }

        public override string ToString() => "UnitSelectionPhrase";
    }
}