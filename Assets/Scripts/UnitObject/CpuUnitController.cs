using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Battle.Map;
using Game.UI;
using Game.Unit;
using Game.Unit.Skill;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Battle
{
    public enum SkillTypeTag { Attack, Buff, Debuff, Heal, Move }
    
    /**
     * Interface for using CpuUnitAI
     */
    [RequireComponent(typeof(UnitObject))]
    public class CpuUnitController : MonoBehaviour
    {
        [SerializeField] private CpuUnitAI _ai;
        public UnitObject unit { get; private set; } 
        
        public bool haveAI => _ai != null;

        private void Awake()
        {
            unit = GetComponent<UnitObject>();
        }

        public void SetAI(CpuUnitAI ai)
        {
            _ai = ai;
        }
        
        public Queue<CpuActionInfo> GetNextActions() => _ai.GetNextActions(unit);
    }
    
    /**
     * Base class of Cpu's AI logic
     */
    public abstract class CpuUnitAI : ScriptableObject
    {
        [SerializeField] protected BattleService _battleService;
        public abstract Queue<CpuActionInfo> GetNextActions(UnitObject unit);
    }
    
    /**
     * ImmutableObject represent the Cpu's Action generated by its AI logic
     */
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
