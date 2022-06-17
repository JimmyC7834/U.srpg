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
        public override bool castableOn(BattleBoardTile tile) => tile.unitOnTile != null;
        public override IEnumerator Cast(BattleService battleService, SkillCastInfo skillCastInfo, SkillCaster.SelectionInfo selectionInfo)
        {
            DamageSourceInfo sourceInfo = DamageSourceInfo.From(skillCastInfo);
            DamageInfo damageInfo = DamageInfo.From(sourceInfo, skillCastInfo.casterTile);
            
            yield return null;
            
            damageInfo.AddModifier(new UnitStatModifier(UnitStatType.DUR, -1, BaseStatModifier.ModifyType.Flat, sourceInfo));
            skillCastInfo.target.DealDamage(damageInfo);
        }
    }
}