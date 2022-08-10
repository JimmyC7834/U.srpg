using System;
using System.Collections;
using System.Collections.Generic;
using Game.Unit;
using UnityEngine;

namespace Game.Battle
{
    public abstract class CpuUnitController : ScriptableObject
    {
        [SerializeField] protected BattleService _battleService;
        [SerializeField] protected SkillCastInfo _skillCastInfo;
        public abstract void GetNextAction(UnitObject unit);
    }
    
    public class PassiveCpuUnitController : CpuUnitController
    {
        public override void GetNextAction(UnitObject unit)
        {
            unit.partTree.GetAllSkills();
            
            // _skillCastInfo.SetCastedSkill();
            SkillCaster skillCaster = new SkillCaster(_battleService, _skillCastInfo);
            
        }
    }
}