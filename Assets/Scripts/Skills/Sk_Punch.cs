using System.Collections;
using System.Collections.Generic;
using Game.Battle.Map;
using UnityEngine;

namespace Game.Unit.Skill
{
    [CreateAssetMenu(menuName = "Game/Skill/Punch", fileName = "Sk_Punch")]
    public class Sk_Punch : SkillSO
    {
        public override bool castableOn(BattleBoardTile tile) => tile.unitOnTile != null;
    }
}