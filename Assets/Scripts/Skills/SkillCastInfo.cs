using System.Collections;
using System.Collections.Generic;
using Game.Battle.Map;
using Game.Unit;
using Game.Unit.Skill;
using UnityEngine;

namespace Game.Battle
{
    /**
     * Immutable object with info of a single skill cast
     */
    public class SkillCastInfo
    {
        public SkillCastInfo(BattleBoardTile _casterTile, SkillSO skill)
        {
            casterTile = _casterTile;
            castedSkill = skill;
        }
        
        public BattleBoardTile casterTile { get; }
        public BattleBoardTile targetTile { get; private set; }
        public SkillSO castedSkill { get; }

        public UnitObject target => targetTile.unitOnTile;
        public UnitObject caster => casterTile.unitOnTile;
 
        public void SetTargetTile(BattleBoardTile tile) => targetTile = tile;
    }
}