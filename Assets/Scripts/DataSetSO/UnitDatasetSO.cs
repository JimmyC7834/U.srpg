using UnityEngine;

namespace Game.DataSet
{
    public enum UnitId
    {
        Norm,
        Norm1,
        Norm2,
        Norm3,
        Count
    }

    [CreateAssetMenu(menuName = "Game/DataSet/Unit")]
    public class UnitDatasetSO : DataSetSO<UnitId, Unit.UnitSO> { }
}