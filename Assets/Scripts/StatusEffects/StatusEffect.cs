namespace Game.Unit.StatusEffect
{
    // TODO: decide if value SEs should be static
    public abstract class StatusEffect
    {
        public abstract StatusEffectId ID { get; }
        protected readonly UnitObject _unit;

        protected StatusEffect(UnitObject unit)
        {
            _unit = unit;
        }

        public abstract void StackEffect(StatusEffect se);

        public virtual void OnRegister() { }
        public virtual void OnRemove() { }
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

    public abstract class TurnCountDownSE : StatusEffect
    {
        public int Count { get; private set; }

        protected TurnCountDownSE(UnitObject unit, int count) : base(unit)
        {
            Count = count;
        }

        public override void StackEffect(StatusEffect se)
        {
            Count += ((TurnCountDownSE) se).Count;
        }

        public override void OnTurnEnd()
        {
            Count--;
            if (Count == 0)
            {
                _unit.RemoveStatusEffect(ID);
                return;
            }

            OnCountDown();
        }

        protected abstract void OnCountDown();
    }
    
    public abstract class MomentCountDownSE : StatusEffect
    {
        public int Count { get; private set; }

        protected MomentCountDownSE(UnitObject unit, int count) : base(unit)
        {
            Count = count;
        }

        public override void StackEffect(StatusEffect se)
        {
            Count += ((TurnCountDownSE) se).Count;
        }

        public override void OnMomentEnd()
        {
            Count--;
            if (Count == 0)
            {
                _unit.RemoveStatusEffect(ID);
                return;
            }

            OnCountDown();
        }

        protected abstract void OnCountDown();
    }

    public abstract class StackableSE : StatusEffect
    {
        public int Count { get; private set; }
        protected StackableSE(UnitObject unit, int count) : base(unit)
        {
            Count = count;
        }
        
        public override void StackEffect(StatusEffect se)
        {
            Count += ((TurnCountDownSE) se).Count;
        }
    }
    
    public abstract class SpecialSE : StatusEffect
    {
        protected SpecialSE(UnitObject unit) : base(unit) { }
        
        public override void StackEffect(StatusEffect se) { }
    }
}