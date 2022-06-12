using System.Collections;
using System.Collections.Generic;
using Game.Battle.Map;
using UnityEngine;

using Game.Unit;
using Game.Unit.Skill;

namespace Game.Battle
{
    [CreateAssetMenu(menuName = "Game/Battle/Data")]
    public class BattleData : ScriptableObject
    {
        // ---MANAGERS---
        public UnitManager unitManager;
        public CursorController cursor;
        public UIManager uiManager;
        public BattleMap _battleMap;
        public MapHighlighter _mapHighlighter;
        
        // ---BATTLEDATA---
        public UnitObject currentUnit { get; private set; }
        
        // ---CASTINGDATA---
        public UnitObject target  { get; private set; }
        public SkillSO castedSkill { get; private set; }

        public void SetCurrentUnit(UnitObject unitObject) => currentUnit = unitObject;
        public void SetCastedSkill(SkillSO skill) => castedSkill = skill;
    }
}