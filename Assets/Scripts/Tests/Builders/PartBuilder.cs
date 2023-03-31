using Game.Unit;
using Game.Unit.Ability;
using Game.Unit.Part;
using Game.Unit.Skill;

namespace Tests.Builders
{
    public class PartBuilder
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

        public UnitPart Build()
        {
            return new UnitPart(_id, _abilityRegs, _skillID, _paramBoostEntries);
        }
    }

    public class TestAbilityBuilder
    {
        private int _count;

        public TestAbilityBuilder WithCount(int value)
        {
            _count = value;
            return this;
        }

        public UnitAbility Build()
        {
            return new TestAbility(_count);
        }

        public static implicit operator UnitAbility(TestAbilityBuilder builder)
        {
            return builder.Build();
        }

        public class TestAbility : UnitAbility
        {
            public override AbilityID ID { get => AbilityID.AB_DEBUG; }
            public TestAbility(int count) : base(count) { }
        }
    }
}