using Game.Battle;
using Game.Battle.Map;
using Game.Unit;
using Game.Unit.Skill;

namespace Game
{
    /**
     * Mutable struct of info of a single attack preformed by an unit
     */
    public class AttackInfo
    {
        public AttackSourceInfo source { get; private set; }
        public BattleBoardTile targetTile { get; private set; }
        public UnitObject target { get => targetTile.unitOnTile; }
        public DamageInfo damageInfo { get; private set; }
        public int combo { get; private set; }
        public bool isCritical { get; private set; }
        public bool missed { get; private set; }
        public bool dodge { get; private set; }

        public static AttackInfo From(AttackSourceInfo _source, BattleBoardTile _targetTile) =>
            From(_source, _targetTile, 1);

        public static AttackInfo From(AttackSourceInfo _source, BattleBoardTile _targetTile, int _combo) => new AttackInfo()
        {
            source = _source,
            targetTile = _targetTile,
            damageInfo = DamageInfo.From(_source),
            combo = _combo,
            isCritical = false,
            missed = false,
            dodge = false,
        };

        public bool ToCritical() => isCritical = true;
        public bool ToMissed() => missed = true;
        public bool ToDodged() => dodge = true;
        
        public void AddModifier(DamageValueModifier damageValueModifier) => damageInfo.AddModifier(damageValueModifier);

        public UnitParamModifier damageModifier => damageInfo.damageModifier;
    }
    
    /**
     * Immutable struct of info of source of an attack
     */
    public struct AttackSourceInfo
    {
        public BattleBoardTile tile { get; private set; }
        public UnitObject unit { get; private set; }
        public SkillSO skill { get; private set; }

        public static AttackSourceInfo From(SkillCast skillCast) =>
            From(skillCast.casterTile, skillCast.skill);
        
        public static AttackSourceInfo From(BattleBoardTile sourceTile, SkillSO _sourceSkill) => new AttackSourceInfo()
        {
            tile = sourceTile,
            unit = sourceTile.unitOnTile,
            skill = _sourceSkill,
        };
    }
}