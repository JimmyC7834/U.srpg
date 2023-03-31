using Game.DataSet;

namespace Game.Unit.Ability
{
    /// <summary>
    /// Represents a ability of an unit. The ability could be stacked
    /// and operate base on the number of stacks.
    /// 
    /// This is a base class for any ability class to be inherited.
    /// This base class provides various functions invoked on event calls
    /// which the derived class should override as needed.
    /// </summary>
    public abstract class UnitAbility : IUnitEventsListener, IDataId<AbilityID>
    {
        /// <summary>
        /// The derived class should define this its corresponding id
        /// </summary>
        public abstract AbilityID ID { get; }
        /// <summary>
        /// The count of ability stack.
        /// </summary>
        public int Count { get; private set; }
        /// <summary>
        /// The unit object this ability is attached to.
        /// This variable is initialized on construct.
        /// </summary>
        protected UnitObject _unit;

        protected UnitAbility(int count)
        {
            Count = count;
        }

        public void RegisterTo(UnitObject unit)
        {
            _unit = unit;
        }

        /// <summary>
        /// Increase the count of the ability stack.
        /// </summary>
        /// <param name="value"></param>
        public void Stack(int value)
        {
            Count += value;
        }
        
        private void OnRegister() { }
        private void OnRemove() { }
        protected void OnStack() { }
        public virtual void OnActionStart() { }
        public virtual void OnActionEnd() { }
        public virtual void OnTurnStart() { }
        public virtual void OnTurnEnd() { }
        public virtual void OnMomentStart() { }
        public virtual void OnMomentEnd() { }
        public virtual void OnPreAttack(AttackInfo info) { }
        public virtual void OnPostAttack(AttackInfo info) { }
        public virtual void OnAttackMissed(AttackInfo info) { }
        public virtual void OnAttackDodged(AttackInfo info) { }
        public virtual void OnAttackHit(AttackInfo info) { }
        public virtual void OnPreTakeAttack(AttackInfo info) { }
        public virtual void OnPostTakeAttack(AttackInfo info) { }
        public virtual void OnDodgeAttack(AttackInfo info) { }
        public virtual void OnPreTakeDamage(DamageInfo info) { }
        public virtual void OnPostTakeDamage(DamageInfo info) { }
    }
}