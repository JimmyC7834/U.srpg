using Game.Unit.Skill;

namespace Game.Battle
{
    public class SkillSelectionPhrase : Phrase
    {
        public SkillSelectionPhrase(BattlePhraseManager parent) : base(parent) { }
                
        public override void Enter()
        {
            _input.DisableAllInput();
        }

        public override void Start()
        {
            battleService.battleUIManager.EnterSkillSelectionMenu(
                battleService.CurrentUnit,
                SkillConfirmed,
                CancelSkillSelection);
        }

        private void SkillConfirmed(SkillSO skill)
        {
            _parent.Pop();
            _parent.Push(new TargetSelectionPhrase(_parent, skill));
        }

        private void CancelSkillSelection()
        {
            _parent.Pop();
            _parent.Push(new ActionSelectionPhrase(_parent));
        }
    }

}