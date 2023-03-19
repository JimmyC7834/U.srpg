using UnityEngine;

namespace Game.Unit.Ability
{
    [CreateAssetMenu(menuName = "Game/Ability/Ab_Ab_CounterRateUp")]
    public class Ab_CounterRateUp : AbilitySO
    {
        [SerializeField] private float _value;

        public override void RegisterTo(UnitObject unit, UnitObject.UnitPartTree.UnitPartTreeNode node)
        {
            unit.stats.ModifyCounterRate(_value, this);
        }
    }
}