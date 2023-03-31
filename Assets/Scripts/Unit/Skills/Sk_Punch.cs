using System.Collections;
using Game.Battle;
using Game.Battle.Map;
using UnityEngine;

namespace Game.Unit.Skill
{
    [CreateAssetMenu(menuName = "Game/Skill/Punch", fileName = "Sk_Punch")]
    public class Sk_Punch : SkillSO
    {
        [SerializeField] private int _value;
            
        public override bool castableOn(BattleBoardTile tile) => tile.unitOnTile != null;
        public override IEnumerator Cast(BattleService battleService, SkillCast skillCast)
        {
            AttackSourceInfo sourceInfo = AttackSourceInfo.From(skillCast);
            AttackInfo attackInfo = AttackInfo.From(sourceInfo, skillCast.targetTile);
            
            attackInfo.AddModifier(new DamageValueModifier(_value, ParamModifier.ModifyType.Flat));
            skillCast.caster.Attack(attackInfo);
            
            skillCast.caster.Anim.AddAnimationStep(UnitAnimation.Attack1, .5f);
            yield return skillCast.caster.Anim.PlayAnimation();
        }
    }
}