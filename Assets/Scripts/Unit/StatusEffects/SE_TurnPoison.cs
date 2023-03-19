namespace Game.Unit.StatusEffect
{
    public class SE_TurnPoison : TurnCountDownSE
    {
        public override StatusEffectId ID { get => StatusEffectId.TurnPoison; }
        private static readonly float _damagePerTurn = 5f;

        public SE_TurnPoison(UnitObject unit, int count) : base(unit, count) { }
        public override StatusEffect Copy() => new SE_TurnPoison(_unit, Count);

        protected override void OnCountDown() => DealDamage();

        public void DealDamage()
        {
            DamageInfo damageInfo = DamageInfo.From(this);
            damageInfo.AddModifier(new DamageValueModifier(_damagePerTurn, ParamModifier.ModifyType.Flat));
            _unit.TakeDamage(damageInfo);
        }
    }
    
    public class SE_MomentPoison : MomentCountDownSE
    {
        public override StatusEffectId ID { get => StatusEffectId.MomentPoison; }
        private static readonly float _damagePerTurn = 1f;

        public SE_MomentPoison(UnitObject unit, int count) : base(unit, count) { }
        public override StatusEffect Copy() => new SE_MomentPoison(_unit, Count);
        
        protected override void OnCountDown() => DealDamage();

        private void DealDamage()
        {
            DamageInfo damageInfo = DamageInfo.From(this);
            damageInfo.AddModifier(new DamageValueModifier(_damagePerTurn, ParamModifier.ModifyType.Flat));
            _unit.TakeDamage(damageInfo);
        }
    }
}