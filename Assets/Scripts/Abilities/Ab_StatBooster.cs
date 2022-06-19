using System.Collections;
using System.Collections.Generic;
using Game.Unit.Ability;
using UnityEngine;

namespace Game.Unit.Ability
{
    [CreateAssetMenu(menuName = "Game/Abilities/Ab_StatBooster", fileName = "Ab_StatBooster")]
    public class Ab_StatBooster : AbilitySO
    {
        [SerializeField] private UnitStat.StatBoostEntry[] _statBoostEntries;
        
        public override void RegisterTo(UnitObject unit, UnitObject.UnitPartTree.UnitPartTreeNode node)
        {
            for (int i = 0; i < _statBoostEntries.Length; i++)
            {
                UnitStat.StatBoostEntry entry = _statBoostEntries[i];
                unit.unitParam.AddModifier(new UnitStatModifier(entry.unitStatType, entry.value, BaseStatModifier.ModifyType.Flat, null));
            }
        }
    }
}