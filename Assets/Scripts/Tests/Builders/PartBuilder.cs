using Game.Unit;
using Game.Unit.Ability;
using Game.Unit.Part;
using Game.Unit.Skill;

namespace Tests.Builders
{
    public class PartBuilder : SimpleBuilder<UnitPart>
    {
        private PartID _id;
        private SkillID _skillID;
        private UnitAbility[] _abilityRegs;
        private UnitParam.ParamBoostEntry[] _paramBoostEntries;

        public PartBuilder WithId(PartID id)
        {
            _id = id;
            return this;
        }
        
        public PartBuilder WithSkill(SkillID id)
        {
            _skillID = id;
            return this;
        }

        public PartBuilder WithAbilities(UnitAbility[] abRegs)
        {
            _abilityRegs = abRegs;
            return this;
        }

        public PartBuilder WithParams(UnitParam.ParamBoostEntry[] paramBoostEntries)
        {
            _paramBoostEntries = paramBoostEntries;
            return this;
        }

        protected override UnitPart Build()
        {
            return new UnitPart(_id, _abilityRegs, _skillID, _paramBoostEntries);
        }
    }
}