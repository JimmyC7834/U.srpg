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
        
        public override bool castableOn(BattleBoardTile tile) => tile.HaveUnitOnTop;

        public override IEnumerator Cast(BattleService battleService, SkillCastInfo skillCastInfo, SkillCaster.SelectionInfo selectionInfo)
        {
            skillCastInfo.target.RegisterStatusEffects(StatusEffectId.Poison1, _turns);
            yield return null;
        }
    }
}