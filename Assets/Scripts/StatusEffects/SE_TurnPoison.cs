namespace Game.Unit.StatusEffect
{
    public class SE_TurnPoison : TurnCountDownSE
    {
        public override StatusEffectId ID { get => StatusEffectId.TurnPoison; }
        private readonly float _damagePerTurn = 5f;

        public SE_TurnPoison(UnitObject unit, int count) : base(unit, count) { }

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
        private readonly float _damagePerTurn;

        public SE_MomentPoison(UnitObject unit, int count, float damagePerTurn) : base(unit, count) { }
        
        protected override void OnCountDown() => DealDamage();

        private void DealDamage()
        {
            DamageInfo damageInfo = DamageInfo.From(this);
            damageInfo.AddModifier(new DamageValueModifier(_damagePerTurn, ParamModifier.ModifyType.Flat));
            _unit.TakeDamage(damageInfo);
        }
    }
}