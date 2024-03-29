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

        public override IEnumerator Cast(BattleService battleService, SkillCast skillCast)
        {
            skillCast.caster.seHandler.RegisterStatusEffects(new SE_OneWay(this));
            skillCast.caster.anim.AddAnimationStep(UnitAnimation.Attack1, .25f);
            skillCast.caster.anim.StartAnimation();
            skillCast.caster.stats.ChangeAP(-skillCast.caster.stats.AP);
            yield return null;
        }
    }
}