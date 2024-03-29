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
            skillCast.target.seHandler.RegisterStatusEffects(new SE_Poison(10f, _turns, this));
            yield return null;
        }
    }
}