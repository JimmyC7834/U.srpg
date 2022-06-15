using System.Linq;
using System.Collections.Generic;
using Game.Battle.Map;
using UnityEngine;

using Game.Unit;
using Game.Unit.Skill;
using UnityEngine.Serialization;

namespace Game.Battle
{
    // [CreateAssetMenu(menuName = "Game/Battle/Data")]
    // public class BattleData : ScriptableObject
    // {
    //     // ---MANAGERS---
    //     public UnitManager unitManager;
    //     public CursorController cursor;
    //     public UIManager uiManager;
    //     public BattleBoard battleBoard;
    //     public MapHighlighter mapHighlighter;
    //     
    //     // ---BATTLEDATA---
    //     public UnitObject currentUnit { get; private set; }
    //     
    //     // ---CASTINGDATA---
    //     public BattleBoardTile targetTile { get; private set; }
    //     public SkillSO castedSkill { get; private set; }
    //
    //     public void SetCurrentUnit(UnitObject unitObject) => currentUnit = unitObject;
    //     public void SetCastedSkill(SkillSO skill) => castedSkill = skill;
    //     public void SetTargetTile(BattleBoardTile tile) => targetTile = tile;
    // }
}