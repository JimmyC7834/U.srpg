using System.Collections;
using System.Collections.Generic;
using Game.DataSet;
using UnityEditor;
using UnityEngine;

namespace Game.Unit.Skill
{
    [CreateAssetMenu(menuName = "Game/Unit/Skill")]
    public class SkillSO : DataEntrySO<SkillId>
    {
        [SerializeField] private Sprite _icon;
        [SerializeField] private bool _unique;
        
        public Sprite icon { get; }
        public bool unique { get; }
    }
}