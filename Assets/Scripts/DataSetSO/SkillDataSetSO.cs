using System.Collections;
using System.Collections.Generic;
using Game.Unit.Skill;
using UnityEngine;

namespace Game.DataSet
{
    [CreateAssetMenu(menuName = "Game/DataSet/Skill")]
    public class SkillDataSetSO : DataSetSO<SkillId, SkillSO> { }
}