using System.Collections;
using Game.Battle;
using Game.Battle.Map;
using Game.Unit.StatusEffect;
using UnityEngine;

namespace Game.Unit.Skill
{
    [CreateAssetMenu(menuName = "Game/Skill/Sk_DealPoison", fileName = "Sk_DealPoison")]
    public class Sk_DealPoison : SkillSO
    {
        [SerializeField] private int _turns;
         
        public override bool castableOn(BattleBoardTile tile) => tile.ContainsUnit;

        public override IEnumerator Cast(BattleService battleService, SkillCast skillCast)
        {
            skillCast.target.AddStatusEffect(
                new SE_TurnPoison(skillCast.target, _turns));
            yield return null;
        }
    }
}