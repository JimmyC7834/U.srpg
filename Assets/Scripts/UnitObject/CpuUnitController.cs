using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Battle.Map;
using Game.Unit;
using Game.Unit.Skill;
using UnityEngine;

namespace Game.Battle
{
    public enum SkillTypeTag { Attack, Buff, Debuff, Heal, Move }
    
    public abstract class CpuUnitController : ScriptableObject
    {
        [SerializeField] protected BattleService _battleService;
        [SerializeField] protected SkillCastInfo _skillCastInfo;
        protected static SkillCaster _skillCaster;
        public abstract void GetNextAction(UnitObject unit);
    }
    
    public class PassiveCpuUnitController : CpuUnitController
    {
        public override void GetNextAction(UnitObject unit)
        {
            if (_skillCaster == null)
                _skillCaster = new SkillCaster(_battleService);
            
            List<SkillSO> skills = unit.partTree.GetAllSkills();
            IEnumerable<SkillSO> atkSkills = skills.Where(sk => sk.IsTagged(SkillTypeTag.Attack));
            SkillSO movSkill = skills.FirstOrDefault(sk => sk.IsTagged(SkillTypeTag.Move));
            
            foreach (SkillSO skill in atkSkills)
            {
                SkillCastInfo skillCastInfo = new SkillCastInfo(
                    _battleService.battleBoard.GetTile(unit.location),
                    skill
                );
                _skillCaster.Initialize(skillCastInfo);
            }
        }
        
        public SkillCaster.SelectionInfo GetRangeTilesFrom(
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
                    if (battleBoard.CoordOnBoard(coord))
                        rangeTiles.Add(battleBoard.GetTile(coord));
                }
            }

            return SkillCaster.SelectionInfo.From(rangeTiles, tileParents);
        }

        // public int ASDist(Vector2 v1, Vector2 v2) => (int) (10 * Vector2.Distance(v1, v2));
        //
        // public void ASPathFind(BattleBoardTile startTile, BattleBoardTile targetTile, bool ignoreTerrain)
        // {
        //     SortedSet<ASNodeInfo> open = new SortedSet<ASNodeInfo>();
        //     HashSet<BattleBoardTile> openTiles = new HashSet<BattleBoardTile>();
        //     HashSet<BattleBoardTile> closed = new HashSet<BattleBoardTile>();
        //
        //     Dictionary<BattleBoardTile, ASNodeInfo> costDict = new Dictionary<BattleBoardTile, ASNodeInfo>();
        //
        //     ASNodeInfo startNodeInfo = new ASNodeInfo(startTile, 0, ASDist(targetTile.coord, startTile.coord));
        //     open.Add(startNodeInfo);
        //     openTiles.Add(startTile);
        //     costDict.Add(startTile, startNodeInfo);
        //
        //     while (true)
        //     {
        //         BattleBoardTile current = open.Min.tile;
        //         openTiles.Remove(open.Min.tile);
        //         open.Remove(open.Min);
        //         closed.Add(current);
        //
        //         if (current.Equals(targetTile))
        //         {
        //             return;
        //         }
        //
        //         foreach (Vector2 tileCoord in current.neighbours)
        //         {
        //             BattleBoardTile neighbour = _battleService.battleBoard.GetTile(tileCoord);
        //             if (closed.Contains(neighbour) || !neighbour.walkable || neighbour.unitOnTile)
        //                 continue;
        //
        //             int cost = ignoreTerrain ? 10 : neighbour.cost * 10;
        //             int hCost = ASDist(targetTile.coord, neighbour.coord);
        //             
        //             if (!costDict.ContainsKey(neighbour))
        //             {
        //                 costDict.Add(neighbour, 
        //                     new ASNodeInfo(neighbour, cost + costDict[current].gCost, hCost));
        //             }
        //
        //             if (cost + costDict[current].fCost < costDict[neighbour].fCost || !openTiles.Contains(neighbour))
        //             {
        //                 costDict[neighbour].gCost = cost + costDict[current].fCost;
        //                 if (openTiles.Contains(neighbour))
        //                     openTiles.Add(neighbour);
        //             }
        //         }
        //     }
        // }
        //
        // private class ASNodeInfo : IComparable<ASNodeInfo>
        // {
        //     public BattleBoardTile tile { get; private set; }
        //     public int gCost; // dist from starting node
        //     public int hCost;  // dist from target node
        //     public int fCost => gCost + hCost;
        //
        //     public ASNodeInfo(BattleBoardTile _tile, int _gCost, int _hCost)
        //     {
        //         // _gCost = (int) (10 * Vector2.Distance(startTile.coord, tile.coord));
        //         // _hCost = (int) (10 * Vector2.Distance(targetTile.coord, tile.coord))
        //         tile = _tile;
        //         gCost = _gCost;
        //         hCost = _hCost;
        //     }
        //
        //     public int CompareTo(ASNodeInfo other) => other.fCost - fCost;
        // }
    }
}
