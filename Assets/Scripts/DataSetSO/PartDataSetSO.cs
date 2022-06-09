using Game.Unit.Part;
using UnityEngine;

namespace Game.DataSet
{
    [CreateAssetMenu(menuName = "Game/DataSet/Part")]
    public class PartDataSetSO : DataSetSO<PartId, PartSO> { }
}