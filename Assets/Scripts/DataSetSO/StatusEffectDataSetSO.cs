using System.Collections;
using System.Collections.Generic;
using Game.Unit.StatusEffects;
using UnityEngine;

namespace Game.DataSet
{
    [CreateAssetMenu(menuName = "Game/DataSet/StatusEffect")]
    public class StatusEffectDataSetSO : DataSetSO<StatusEffectId, StatusEffectSO> { }
}