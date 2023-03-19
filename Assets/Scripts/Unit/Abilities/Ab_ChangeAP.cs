using UnityEngine;

namespace Game.Unit.Ability
{
    [CreateAssetMenu(menuName = "Game/Ability/Ab_ChangeAP")]
    public class Ab_ChangeAP : AbilitySO
    {
        [SerializeField] private int value;
        
        public override void RegisterTo(UnitObject unit, UnitObject.UnitPartTree.UnitPartTreeNode node)
        {
            unit.OnAbTurnChanged += ChangeAP;
        }

        public void ChangeAP(UnitObject unit) => unit.stats.ChangeAP(value);
    }
}