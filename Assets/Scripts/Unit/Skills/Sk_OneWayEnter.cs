using System.Collections;
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
            skillCast.caster.AddStatusEffect(new SE_OneWay(skillCast.caster));
            skillCast.caster.Anim.AddAnimationStep(UnitAnimation.Attack1, .25f);
            skillCast.caster.Anim.StartAnimation();
            skillCast.caster.Stats.ChangeAP(-skillCast.caster.Stats.AP);
            yield return null;
        }
    }
}