using Game.Unit;
using UnityEngine;

namespace Game.DataSet
{
    [CreateAssetMenu(menuName = "Game/DataSet/Unit")]
    public class UnitDatasetSO : DataSetSO<UnitId, Unit.UnitSO> { }
}