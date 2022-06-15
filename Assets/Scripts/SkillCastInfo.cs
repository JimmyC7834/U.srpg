using System.Collections;
using System.Collections.Generic;
using Game.Battle.Map;
using Game.Unit.Skill;
using UnityEngine;

namespace Game.Battle
{
    [CreateAssetMenu(menuName = "Game/Battle/SkillCastInfo")]
    public class SkillCastInfo : ScriptableObject
    {
        public BattleBoardTile casterTile { get; private set; }
        public BattleBoardTile targetTile { get; private set; }
        public SkillSO castedSkill { get; private set; }

        public void SetCasterTile(BattleBoardTile tile) => casterTile = tile;
        public void SetTargetTile(BattleBoardTile tile) => targetTile = tile;
        public void SetCastedSkill(SkillSO skill) => castedSkill = skill;
    }
}