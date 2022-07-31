using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Battle;
using Game.Battle.Map;
using Game.Unit;
using Game.Unit.Skill;
using UnityEngine;

namespace Game.Battle
{
    public class SkillCaster
    {
        private BattleService _battleService;
        private SkillCastInfo _skillCastInfo;
    
        public struct SelectionInfo
        {
            public HashSet<BattleBoardTile> rangeTiles { get; private set; }
            public Dictionary<Vector2, Vector2> tileParents { get; private set; }
            public IEnumerable<Vector2> rangeV2 { get; private set; }
                    
            public static SelectionInfo From(HashSet<BattleBoardTile> _rangeTiles, Dictionary<Vector2, Vector2> _tileParents) => new SelectionInfo
            {
                rangeTiles = _rangeTiles,
                rangeV2 = _rangeTiles.Select(x => x.coord),
                tileParents = _tileParents,
            };
        }
    
        private SelectionInfo _selectionInfo;

        public SkillCaster(BattleService battleService, SkillCastInfo skillCastInfo)
        {
            _battleService = battleService;
            _skillCastInfo = skillCastInfo;

            _skillCastInfo.SetCasterTile(_battleService.CurrentTile);

            UnitObject caster = _skillCastInfo.casterTile.unitOnTile;
            SkillSO skill = _skillCastInfo.castedSkill;
            _selectionInfo = GetRangeTilesFrom(
                caster.location.x,
                caster.location.y, 
                skill.calWithMoveRange ? caster.param.GetMoveRange() : skill.range,
                skill.ignoreTerrain, 
                skill.includeSelf, 
                skill.optionalRange);


            _battleService.mapHighlighter.HighlightTiles(
                _selectionInfo.rangeTiles.Select(x => x.coord),
                TileHighlightColor.InTargetRange);
        }

        public bool Castable()
        {
            return (_selectionInfo.rangeV2.Contains(_battleService.CurrentCoord) &&
                    _skillCastInfo.castedSkill.castableOn(_battleService.CurrentTile));
        }

        public void CastSkill(Action callback)
        {
            _skillCastInfo.SetTargetTile(_battleService.CurrentTile);
            _battleService.mapHighlighter.RemoveHighlights();
            // TODO: phrase for animation
            _skillCastInfo.castedSkill.StartCasting(_battleService, _skillCastInfo, _selectionInfo, callback);
        }
    
        private SelectionInfo GetRangeTilesFrom(
            int gx, int gy, int totalRange, bool ignoreTerrain, bool includeSelf, Optional<Vector2[]> optionalRange)
        {
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
            tileCostLeft.Add(startTile.coord, totalRange);

            while (queue.Count != 0)
            {
                BattleBoardTile currentTile = queue.Dequeue();
                rangeTiles.Add(currentTile);
                foreach (Vector2 tileCoord in battleBoard.GetTile(currentTile.x, currentTile.y).neighbours)
                {
                    BattleBoardTile tile = battleBoard.GetTile(tileCoord);
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
                    Vector2 coord = v + _battleService.CurrentCoord;
                    if (battleBoard.CoordOnBoard(coord))
                        rangeTiles.Add(battleBoard.GetTile(coord));
                }
            }

            return SelectionInfo.From(rangeTiles, tileParents);
        }
    }
}