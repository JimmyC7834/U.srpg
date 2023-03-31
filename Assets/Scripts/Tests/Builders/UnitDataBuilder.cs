using Game.Unit;
using Game.Unit.Part;

namespace Tests.Builders
{
    public class UnitDataBuilder : SimpleBuilder<UnitData>
    {
        private UnitPart[] _parts;

        public UnitDataBuilder WithParts(UnitPart[] parts)
        {
            _parts = parts;
            return this;
        }

        protected override UnitData Build()
        {
            return new UnitData(_parts);
        }
    }
}