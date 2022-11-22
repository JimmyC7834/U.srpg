using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Battle.Map;
using Game.Unit;
using Game.Unit.Skill;
using UnityEngine;

namespace Game.Battle
{
     [CreateAssetMenu(fileName = "PassiveCpuUnitController", menuName = "Game/AI/Passive")]
    public class PassiveCpuUnitController : CpuUnitAI
    {
        public override Queue<CpuActionInfo> GetNextActions(UnitObject unit)
        {
            List<SkillSO> skills = unit.partTree.GetAllSkills();
            IEnumerable<SkillSO> atkSkills = skills.Where(sk => sk.IsTagged(SkillTypeTag.Attack));
            SkillSO movSkill = skills.FirstOrDefault(sk => sk.IsTagged(SkillTypeTag.Move));
            List<SkillCast.SelectionRange> selections = new List<SkillCast.SelectionRange>();
            Dictionary<UnitObject, List<SkillSO>> skillForUnit = new Dictionary<UnitObject, List<SkillSO>>();
            
            // search through all attack skill's range (included move range) for targets
            foreach (SkillSO skill in atkSkills)
            {
                SkillCast.SelectionRange selection = GetRangeTilesFrom(unit.gridX, unit.gridY, movSkill, skill);
                List<UnitObject> unitsInRange = selection.rangeTiles.Where(tile => tile.unitOnTile != null).Select(tile => tile.unitOnTile).ToList();
                if (unitsInRange.Contains(unit)) unitsInRange.Remove(unit);
                if (unitsInRange.Count == 0) continue;
                foreach (UnitObject unitInRange in unitsInRange)
                {
                    if (!skillForUnit.ContainsKey(unitInRange))
                        skillForUnit.Add(unitInRange, new List<SkillSO>());
                    skillForUnit[unitInRange].Add(skill);
                }
            }
            
            if (skillForUnit.Count == 0) return new Queue<CpuActionInfo>();
            UnitObject target = skillForUnit.Keys.First();
            SkillSO skillToUse = skillForUnit[target][Random.Range(0, skillForUnit[target].Count)];

            Queue<CpuActionInfo> actions = new Queue<CpuActionInfo>();

            PathFinder pathFinder = new PathFinder(_battleService.battleBoard);
            List<PathFinder.AStarNode> path = 
                pathFinder.AStar(unit.gridX, unit.gridY, target.gridX, target.gridY);
            
            // determine which tile to move to be able to cast the skill
            // choose the furthest tile from target 
            for (int i = 0; i < path.Count; i++)
            {
                if (path[i].CostTo(target.gridX, target.gridY) == skillToUse.range)
                {
                    actions.Enqueue(CpuActionInfo.From(
                        _battleService.battleBoard.GetTile(path[i].x, path[i].y), movSkill));
                    break;
                }
            }
            
            actions.Enqueue(CpuActionInfo.From(
                _battleService.battleBoard.GetTile(target.gridX, target.gridY), skillToUse));

            return actions;
        }
        
        public SkillCast.SelectionRange GetRangeTilesFrom(
            int gx, int gy, SkillSO moveSkill, SkillSO skill)
        {
            // basic config
            bool ignoreTerrain = moveSkill.ignoreTerrain;
            Optional<Vector2[]> optionalRange = moveSkill.optionalRange;
            
            // BFS search
            Queue<BattleBoardTile> queue = new Queue<BattleBoardTile>();
            HashSet<Vector2> explored = new HashSet<Vector2>();
            Dictionary<Vector2, float> tileCostLeft = new Dictionary<Vector2, float>();
            Dictionary<Vector2, Vector2> tileParents = new Dictionary<Vector2, Vector2>();
            HashSet<BattleBoardTile> rangeTiles = new HashSet<BattleBoardTile>();

            BattleBoard battleBoard = _battleService.battleBoard;
            BattleBoardTile startTile = battleBoard.GetTile(gx, gy);
            explored.Add(startTile.coord);
            queue.Enqueue(startTile);
            tileCostLeft.Add(startTile.coord, moveSkill.range + skill.range);

            while (queue.Count != 0)
            {
                BattleBoardTile currentTile = queue.Dequeue();
                rangeTiles.Add(currentTile);
                foreach (Vector2 tileCoord in battleBoard.GetTile(currentTile.x, currentTile.y).neighbours)
                {
                    BattleBoardTile tile = battleBoard.GetTile(tileCoord);
                    if (tileCostLeft[currentTile.coord] <= skill.range)
                    {
                        ignoreTerrain = skill.ignoreTerrain;
                        optionalRange = skill.optionalRange;
                    }
                    
                    float costLeft = tileCostLeft[currentTile.coord] - (ignoreTerrain ? 1 : 2);
                    if (!explored.Contains(tileCoord) && costLeft >= 0f && tile.walkable)
                    {
                        queue.Enqueue(tile);
                        explored.Add(tileCoord);

                        if (!tileParents.ContainsKey(tileCoord))
                            tileParents.Add(tileCoord, currentTile.coord);

                        if (!tileCostLeft.ContainsKey(tileCoord))
                            tileCostLeft.Add(tileCoord, costLeft);

                        tileCostLeft[tileCoord] = costLeft;
                    }
                }
            }

            // add optional range
            if (optionalRange.Enabled)
            {
                foreach (Vector2 v in optionalRange.Value)
                {
                    Vector2 coord = v + _battleService.CurrentCoord;
                    if (battleBoard.ContainsCoord(coord))
                        rangeTiles.Add(battleBoard.GetTile(coord));
                }
            }

            return SkillCast.SelectionRange.From(rangeTiles, tileParents);
        }
    }
}
