using Game.Unit.Skill;

namespace Game.Battle
{
    public class TargetSelectionPhrase : Phrase
    {
        private SkillCaster _skillCaster;
        private SkillSO _skill;
        private SkillCastInfo _skillCastInfo;

        public TargetSelectionPhrase(BattlePhraseManager parent, SkillSO skill) : base(parent)
        {
            _skill = skill;
        }

        public override void Enter()
        {
            _input.DisableAllInput();
            _skillCaster = new SkillCaster(battleService);
            _skillCaster.HighlightRange();
        }

        public override void Start()
        {
            _input.EnableMapNaviInput();
            _cursor.OnConfirm += OnConfirm;
        }

        private void OnConfirm(CursorController cursor)
        {
            if (!_skillCaster.Castable()) return;
            _cursor.OnConfirm -= OnConfirm;
                    
            _input.DisableAllInput();
            _parent.Pop();
            SkillAnimationPhrase skillAnimationPhrase = new SkillAnimationPhrase(_parent);
            _parent.Push(skillAnimationPhrase);
            
            _skillCastInfo = new SkillCastInfo(battleService.CurrentTile, _skill);
            _skillCaster.Initialize(_skillCastInfo);
            _skillCastInfo.SetTargetTile(battleService.CurrentTile);
            
            _skillCaster.CastSkill(skillAnimationPhrase.EndPhrase);
        }
    }
}