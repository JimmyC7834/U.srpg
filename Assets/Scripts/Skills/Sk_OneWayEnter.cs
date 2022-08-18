using System.Collections;
using System.Collections.Generic;
using Game.Battle;
using Game.Battle.Map;
using Game.Unit.StatusEffect;
using UnityEngine;

namespace Game.Unit.Skill
{
    [CreateAssetMenu(menuName = "Game/Skill/Sk_OneWayEnter", fileName = "Sk_OneWayEnter")]
    public class Sk_OneWayEnter : SkillSO
    {
        public override bool castableOn(BattleBoardTile tile) => tile.unitOnTile != null;

        public override IEnumerator Cast(BattleService battleService, SkillCastInfo skillCastInfo, SkillCaster.SelectionInfo selectionInfo)
        {
            skillCastInfo.caster.RegisterStatusEffects(new SE_OneWay(), this);
            skillCastInfo.caster.anim.AddAnimationStep(UnitAnimation.Attack1, .25f);
            skillCastInfo.caster.anim.StartAnimation();
            skillCastInfo.caster.param.ChangeMP(-skillCastInfo.caster.param.MP);
            yield return null;
        }
    }
}