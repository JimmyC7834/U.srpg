using System.Collections;
using System.Collections.Generic;
using Game.Battle;
using Game.Battle.Map;
using UnityEngine;

namespace Game.Unit.Skill
{
    [CreateAssetMenu(menuName = "Game/Skill/Move", fileName = "Sk_Move")]
    public class Sk_Move : SkillSO
    {
        public override bool castableOn(BattleBoardTile tile) => tile.walkable && tile.unitOnTile == null;

        // public override IEnumerator StartCasting(BattleData battleData, SkillCaster.SelectionInfo selectionInfo)
                    // {
                    //     
                    // }
    }
}