using UnityEngine;

namespace Game.DataSet
{
    public enum UnitId
    {
        Norm,
        Count
    }

    [CreateAssetMenu(menuName = "Game/DataSet/Unit")]
    public class UnitDatasetSO : DataSetSO<UnitId, Unit.UnitSO> { }
}