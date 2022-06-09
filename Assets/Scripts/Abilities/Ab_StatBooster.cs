using System.Collections;
using System.Collections.Generic;
using Game.Unit.Ability;
using UnityEngine;

namespace Game.Unit.Ability
{
    [CreateAssetMenu(menuName = "Game/Abilities/Ab_StatBooster", fileName = "Ab_StatBooster")]
    public class Ab_StatBooster : AbilitySO
    {
        public override void RegisterTo(UnitObject unit, UnitObject.UnitPartTree.UnitPartTreeNode node)
        {
            unit.unitParam.AddModifier(
                new UnitStatModifier(
                    UnitParam.UnitStatType.DUR,
                    0.05f,
                    BaseStatModifier.ModifyType.PercentAdd,
                    node
                )
            );
        }
    
        public override void OnTrigger()
        {
        
        }
    }
}