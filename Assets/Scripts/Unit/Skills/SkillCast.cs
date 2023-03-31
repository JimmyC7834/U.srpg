using System;
using System.Collections.Generic;
using System.Linq;
using Game.Battle;
using Game.Battle.Map;
using UnityEngine;

namespace Game.Unit.Skill
{
    /**
     * Immutable object with info of a single skill cast
     */
    public class SkillCast
    {
        public SkillCast(BattleBoard board, BattleBoardTile _casterTile, SkillSO _skill)
        {
            casterTile = _casterTile;
            skill = _skill;
            selectionRange = CalculateSelectionRange(
                board,
                caster.Location.x,
                caster.Location.y, 
                skill.calWithMoveRange ? caster.Stats.GetMoveRange() : skill.range,
                skill.ignoreTerrain, 
                skill.includeSelf, 
                skill.optionalRange);
        }

        public readonly BattleBoardTile casterTile;
        public readonly SkillSO skill;
        public readonly SelectionRange selectionRange;
        public BattleBoardTile targetTile { get; private set; }

        public UnitObject target => targetTile.unitOnTile;
        public UnitObject caster => casterTile.unitOnTile;

        public bool Castable(BattleBoardTile _targetTile)
        {
            return (selectionRange.rangeV2.Contains(_targetTile.coord) &&
                    skill.castableOn(_targetTile));
        }
        
        public void Cast(BattleService _battleService, BattleBoardTile _targetTile, Action callback)
        {
            // TODO: phrase for animation
            targetTile = _targetTile;
            // TODO: maybe SkillCastInfo?
            skill.StartCasting(_battleService, this, callback);
        }
        
        public struct SelectionRange
        {
            public HashSet<BattleBoardTile> rangeTiles { get; private set; }
            public Dictionary<Vector2, Vector2> tileParents { get; private set; }
            public IEnumerable<Vector2> rangeV2 { get; private set; }

            public static SelectionRange From(HashSet<BattleBoardTile> _rangeTiles, Dictionary<Vector2, Vector2> _tileParents) => new SelectionRange
            {
                rangeTiles = _rangeTiles,
                rangeV2 = _rangeTiles.Select(x => x.coord),
                tileParents = _tileParents,
            };
        }
        
        private SelectionRange CalculateSelectionRange(
            BattleBoard board, int gx, int gy, int totalRange, bool ignoreTerrain, bool includeSelf, Optional<Vector2[]> optionalRange)
        {
            // BFS search
            Queue<BattleBoardTile> queue = new Queue<BattleBoardTile>();
            HashSet<Vector2> explored = new HashSet<Vector2>();
            Dictionary<Vector2, float> tileCostLeft = new Dictionary<Vector2, float>();
            Dictionary<Vector2, Vector2> tileParents = new Dictionary<Vector2, Vector2>();
            HashSet<BattleBoardTile> rangeTiles = new HashSet<BattleBoardTile>();

            BattleBoardTile startTile = board.GetTile(gx, gy);
            explored.Add(startTile.coord);
            queue.Enqueue(startTile);
            tileCostLeft.Add(startTile.coord, totalRange);

            while (queue.Count != 0)
            {
                BattleBoardTile currentTile = queue.Dequeue();
                rangeTiles.Add(currentTile);
                foreach (Vector2 tileCoord in board.GetTile(currentTile.x, currentTile.y).neighbours)
                {
                    BattleBoardTile tile = board.GetTile(tileCoord);
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

            if (!includeSelf)
                rangeTiles.Remove(startTile);
        
            // add optional range
            if (optionalRange.Enabled)
            {
                foreach (Vector2 v in optionalRange.Value)
                {
                    Vector2 coord = v + casterTile.coord;
                    if (board.ContainsCoord(coord))
                        rangeTiles.Add(board.GetTile(coord));
                }
            }

            return SelectionRange.From(rangeTiles, tileParents);
        }

    }
}