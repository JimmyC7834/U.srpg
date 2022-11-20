namespace Game
{
    /**
     * Immutable modifier used to modify ModifiableParam.
     */
    public class ParamModifier
    {
        /**
         * type of modification assigned with priority.
         */
        public enum ModifyType
        {
            Flat = 100,
            PercentAdd = 200,
            Percent = 300,
        }

        public readonly float value;
        public readonly ModifyType type;

        /**
         * modifiers with lower priority value are calculated first.
         */
        public readonly int priority;
        public readonly object source;

        public ParamModifier(float _value, ModifyType _type, int _priority, object _source)
        {
            value = _value;
            type = _type;
            priority = _priority;
            source = _source;
        }

        public ParamModifier(float _value, ModifyType _type) : this(_value, _type, (int)_type, null) { }

        public ParamModifier(float _value, ModifyType _type, int _priority) : this(_value, _type, _priority, null) { }

        public ParamModifier(float _value, ModifyType _type, object _source) : this(_value, _type, (int)_type, _source) { }
    }
}