using Game.Unit;

namespace Game
{
    /**
     * Mutable struct of single damage deal
     */
    public struct DamageInfo
    {
        public DamageValue DamageValue { get; private set; }
        public object source { get; private set; }
        
        public void AddModifier(DamageValueModifier damageValueModifier) => 
            DamageValue.AddModifier(damageValueModifier);
        
        /**
         * Generate final UnitStatModifier to modify DUR base on the damage value
         */
        public UnitParamModifier damageModifier => 
            new UnitParamModifier(UnitStatType.DUR, -DamageValue.Value, ParamModifier.ModifyType.Flat, source);
        
        public static DamageInfo From(object _source) => new DamageInfo()
        {
            DamageValue = new DamageValue(),
            source = _source,
        };
    }
}