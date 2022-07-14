using System.Collections;
using System.Collections.Generic;
using Game.Battle;
using Game.Battle.Map;
using Game.Unit.StatusEffects;
using UnityEngine;

namespace Game.Unit.Skill
{
    [CreateAssetMenu(menuName = "Game/Skill/Sk_DealPoison", fileName = "Sk_DealPoison")]
    public class Sk_DealPoison : SkillSO
    {
        public override bool castableOn(BattleBoardTile tile) => tile.HaveUnitOnTop;

        public override IEnumerator Cast(BattleService battleService, SkillCastInfo skillCastInfo, SkillCaster.SelectionInfo selectionInfo)
        {
            skillCastInfo.target.RegisterStatusEffects(StatusEffectId.Poison1);
            yield return null;
        }
    }
}