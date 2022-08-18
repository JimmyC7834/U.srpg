using System.Collections;
using System.Collections.Generic;
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
        public override IEnumerator Cast(BattleService battleService, SkillCastInfo skillCastInfo, SkillCaster.SelectionInfo selectionInfo)
        {
            AttackSourceInfo sourceInfo = AttackSourceInfo.From(skillCastInfo);
            AttackInfo attackInfo = AttackInfo.From(sourceInfo, skillCastInfo.targetTile);

            attackInfo.AddModifier(new DamageStatModifier(_value, BaseStatModifier.ModifyType.Flat));
            skillCastInfo.caster.DealDamageTo(attackInfo);
            
            skillCastInfo.caster.anim.AddAnimationStep(UnitAnimation.Attack1, 0.5f);
            skillCastInfo.caster.anim.StartAnimation();
            yield return null;
        }
    }
}