using System;
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
        [SerializeField] private float movingTimePerTile;
        
        public override bool castableOn(BattleBoardTile tile) => tile.walkable && tile.unitOnTile == null;

        public override IEnumerator Cast(BattleService battleService, SkillCastInfo skillCastInfo, SkillCaster.SelectionInfo selectionInfo)
        {
            
            UnitObject casterObject = skillCastInfo.casterTile.unitOnTile;
            battleService.battleBoard.MoveUnitFromTo(skillCastInfo.casterTile, skillCastInfo.targetTile);
            
            battleService.logConsole.SendText($"{casterObject.displayName} moved from {skillCastInfo.casterTile.coord} to {skillCastInfo.targetTile.coord}");

            PathFinder pathFinder = new PathFinder(battleService.battleBoard);
            List<PathFinder.PathFindNode> pathNodes = pathFinder.FindPath(casterObject.gridX, casterObject.gridY, 
                skillCastInfo.targetTile.x, skillCastInfo.targetTile.y);

            // Stack<Vector2> path = new Stack<Vector2>();
            // path.Push(skillCastInfo.targetTile.coord);
            //
            // while (selectionInfo.tileParents.ContainsKey(path.Peek()))
            // {
            //     path.Push(selectionInfo.tileParents[path.Peek()]);
            // }
            //
            // casterObject.anim.SwitchStateTo(UnitAnimation.Move);
            //
            // while (path.Count != 0)
            // {
            //     yield return casterObject.anim.MoveUnitOnBroad(path.Pop(), movingTimePerTile);
            // }
            
            casterObject.anim.SwitchStateTo(UnitAnimation.Move);
            
            while (pathNodes.Count != 0)
            {
                Vector2 targetPos = new Vector2(pathNodes[0].x, pathNodes[0].y);
                pathNodes.RemoveAt(0);
                yield return casterObject.anim.MoveUnitOnBroad(targetPos, movingTimePerTile);
            }
            
            casterObject.anim.SwitchStateTo(UnitAnimation.Idle);
        }
    }
}