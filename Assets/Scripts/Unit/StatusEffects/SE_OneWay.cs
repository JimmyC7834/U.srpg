namespace Game.Unit.StatusEffect
{
    public class SE_OneWay : SpecialSE
    {
        public override StatusEffectId ID { get => StatusEffectId.OneWay; }
        private int _mpLeft;

        public SE_OneWay(UnitObject unit) : base(unit) { }

        public override StatusEffectRegister Copy()
        {
            SE_OneWay se = new SE_OneWay(_unit);
            se._mpLeft = _mpLeft;
            return se;
        }

        public override void OnActionEnd()
        {
            _mpLeft = _unit.Stats.AP;
            if (_mpLeft < 0)
                _unit.RemoveStatusEffect(ID);
            
            _unit.Stats.ChangeAP(-_mpLeft);
        }

        public override void OnTurnEnd()
        {
            RecoverMP();
        }

        public void RecoverMP()
        {
            _unit.Stats.ChangeAP(_mpLeft);
        }
    }
}