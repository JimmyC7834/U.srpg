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
    // TODO: rewrite skill cast logic
    // public class SkillCaster
    // {
    //     private BattleService _battleService;
    //     private UnitObject _caster;
    //     private SkillSO _skill;
    //     private SkillCast
    //
    //     private SelectionInfo _selectionInfo;
    //     public SelectionInfo selectionInfo { get => _selectionInfo; }
    //     public HashSet<BattleBoardTile> rangeTiles { get; private set; }
    //     public Dictionary<Vector2, Vector2> tileParents { get; private set; }
    //     public IEnumerable<Vector2> rangeV2 { get; private set; }
    //
    //     public SkillCaster(BattleService battleService, UnitObject caster, SkillSO skill)
    //     {
    //         _battleService = battleService;
    //         _caster = caster;
    //         _skill = skill;
    //     }
    //
    //     public void HighlightRange()
    //     {
    //         _battleService.mapHighlighter.HighlightTiles(
    //             _selectionInfo.rangeTiles.Select(x => x.coord),
    //             TileHighlightColor.InTargetRange);
    //     }
    // }
}