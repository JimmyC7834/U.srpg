namespace Game.Battle
{
    public class SkillAnimationPhrase : Phrase
    {
        public SkillAnimationPhrase(BattlePhraseManager parent) : base(parent) { }

        public override void Enter()
        {
            _input.DisableAllInput();
        }

        public void EndPhrase()
        {
            _parent.Pop();
            _parent.Push(new UnitSelectionPhrase(_parent));
        }
    }
}