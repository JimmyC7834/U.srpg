namespace Tests.Builders
{
    public abstract class SimpleBuilder<T>
    {
        protected abstract T Build();
        public static implicit operator T(SimpleBuilder<T> builder)
        {
            return builder.Build();
        }
    }
    
    public static class A
    {
        public static PartBuilder Part => new PartBuilder();
        public static TestAbilityBuilder TestAbility => new TestAbilityBuilder();
    }
    
    public static class An
    {
        public static UnitDataBuilder UnitData => new UnitDataBuilder();
    }
}