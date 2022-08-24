using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Battle.Map;
using Game.Unit;
using Game.Unit.Skill;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Battle
{
    public enum SkillTypeTag { Attack, Buff, Debuff, Heal, Move }

    public class CpuUnitController : MonoBehaviour
    {
        [SerializeField] private CpuUnitAI _ai;

        public bool haveAI => _ai != null;

        public void SetAI(CpuUnitAI ai)
        {
            _ai = ai;
        }
        
        public List<CpuActionInfo> GetNextActions() => _ai.GetNextActions(GetComponent<UnitObject>());
    }
    
    public abstract class CpuUnitAI : ScriptableObject
    {
        [SerializeField] protected BattleService _battleService;
        protected static SkillCaster _skillCaster;
        public abstract List<CpuActionInfo> GetNextActions(UnitObject unit);
    }

    public struct CpuActionInfo
    {
        public SkillSO skill { get; private set; }
        public BattleBoardTile targetTile { get; private set; }

        public static CpuActionInfo From(BattleBoardTile _targetTile, SkillSO _skill) => new CpuActionInfo()
        {
            skill = _skill,
            targetTile = _targetTile,
        };

        public static CpuActionInfo Empty() => new CpuActionInfo();
    }
}