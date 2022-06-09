using Game.Unit.Ability;
using UnityEngine;

namespace Game.DataSet
{
    [CreateAssetMenu(menuName = "Game/DataSet/Ability")]
    public class AbilityDataSetSO : DataSetSO<AbilityId, Unit.Ability.AbilitySO> { }
}