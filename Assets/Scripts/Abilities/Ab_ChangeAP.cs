using System;
using System.Collections;
using System.Collections.Generic;
using Game.Unit.Ability;
using UnityEngine;

namespace Game.Unit.Ability
{
    [CreateAssetMenu(menuName = "Game/Ability/Ab_ChangeAP")]
    public class Ab_ChangeAP : AbilitySO
    {
        [SerializeField] private int value;
        
        public override void RegisterTo(UnitObject unit, UnitObject.UnitPartTree.UnitPartTreeNode node)
        {
            unit.OnTurnChanged += ChangeAP;
        }

        public void ChangeAP(UnitObject unit) => unit.param.ChangeAP(value);
    }
}