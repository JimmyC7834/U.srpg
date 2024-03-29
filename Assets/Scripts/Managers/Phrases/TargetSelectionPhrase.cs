﻿using Game.Battle.Map;
using Game.Unit.Skill;

namespace Game.Battle
{
    public class TargetSelectionPhrase : Phrase
    {
        private SkillCast _skillCast;
        private SkillSO _skill;

        public TargetSelectionPhrase(BattlePhraseManager parent, SkillSO skill) : base(parent)
        {
            _skill = skill;
            _skillCast = new SkillCast(
                battleService.battleBoard,
                battleService.CurrentTile,
                _skill
            );
        }

        public override void Enter()
        {
            _input.DisableAllInput();
            
            battleService.mapHighlighter.HighlightTiles(
                _skillCast.selectionRange.rangeV2,
                TileHighlightColor.InTargetRange
                );
        }

        public override void Start()
        {
            _input.EnableMapNaviInput();
            _cursor.OnConfirm += OnConfirm;
        }

        private void OnConfirm(CursorController cursor)
        {
            BattleBoardTile targetTile = battleService.CurrentTile;
            if (!_skillCast.Castable(targetTile)) return;
            _cursor.OnConfirm -= OnConfirm;
            _input.DisableAllInput();
            
            battleService.mapHighlighter.RemoveHighlights();
            
            _parent.Pop();
            SkillAnimationPhrase skillAnimationPhrase = new SkillAnimationPhrase(_parent);
            _parent.Push(skillAnimationPhrase);
            _skillCast.Cast(battleService, targetTile, skillAnimationPhrase.EndPhrase);
        }
    }
}