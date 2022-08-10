using System;
using System.Collections;
using System.Collections.Generic;
using Game.Battle;
using Game.Battle.Map;
using UnityEngine;

namespace Game.Unit.Skill
{
    [CreateAssetMenu(menuName = "Game/Skill/Move", fileName = "Sk_Move")]
    public class Sk_Move : SkillSO, ISk_Move
    {
        [SerializeField] private float movingTimePerTile;
        
        public override bool castableOn(BattleBoardTile tile) => tile.walkable && tile.unitOnTile == null;

        public override IEnumerator Cast(BattleService battleService, SkillCastInfo skillCastInfo, SkillCaster.SelectionInfo selectionInfo)
        {
            UnitObject casterObject = skillCastInfo.casterTile.unitOnTile;
            battleService.battleBoard.MoveUnitFromTo(skillCastInfo.casterTile, skillCastInfo.targetTile);
            
            Stack<Vector2> path = new Stack<Vector2>();
            path.Push(skillCastInfo.targetTile.coord);

            while (selectionInfo.tileParents.ContainsKey(path.Peek()))
            {
                path.Push(selectionInfo.tileParents[path.Peek()]);
            }
            
            casterObject.anim.SwitchStateTo(UnitAnimation.Move);
            
            while (path.Count != 0)
            {
                yield return casterObject.anim.MoveUnitOnBroad(path.Pop(), movingTimePerTile);
            }
            
            casterObject.anim.SwitchStateTo(UnitAnimation.Idle);
        }
    }
}