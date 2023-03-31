using Game.Unit.Ability;

namespace Tests.Builders
{
    
    public class TestAbilityBuilder : SimpleBuilder<UnitAbility>
    {
        private int _count;

        public TestAbilityBuilder WithCount(int value)
        {
            _count = value;
            return this;
        }

        protected override UnitAbility Build()
        {
            return new TestAbility(_count);
        }

        private class TestAbility : UnitAbility
        {
            public override AbilityID ID { get => AbilityID.AB_DEBUG; }
            public TestAbility(int count) : base(count) { }
        }
    }
}