namespace Game.Unit.StatusEffect
{
    public class SE_OneWay : SpecialSE
    {
        public override StatusEffectId ID { get => StatusEffectId.OneWay; }
        private int _mpLeft;

        public SE_OneWay(UnitObject unit) : base(unit) { }

        public override void OnActionEnd()
        {
            _mpLeft = _unit.stats.AP;
            if (_mpLeft < 0)
                _unit.RemoveStatusEffect(ID);
            
            _unit.stats.ChangeAP(-_mpLeft);
        }

        public override void OnTurnEnd()
        {
            RecoverMP();
        }

        public void RecoverMP()
        {
            _unit.stats.ChangeAP(_mpLeft);
        }
    }
}