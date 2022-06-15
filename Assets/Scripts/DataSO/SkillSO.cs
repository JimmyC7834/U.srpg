using System;
using System.Collections;
using System.Collections.Generic;
using Game.Battle;
using Game.Battle.Map;
using Game.DataSet;
using UnityEditor;
using UnityEngine;

namespace Game.Unit.Skill
{
    [CreateAssetMenu(menuName = "Game/Unit/Skill")]
    public class SkillSO : DataEntrySO<SkillId>
    {
        [SerializeField] private BattleData _battleData;
        [SerializeField] private Sprite _icon;
        [SerializeField] private bool _unique;

        [Serializable]
        private struct SkillSelectionRange
        {
            public int range;
            public bool ignoreTerrain;
            public bool includeSelf;
            public Optional<Vector2[]> optionalRange;
        }

        [SerializeField] private SkillSelectionRange _skillSelectionRange;
        
        public Sprite icon { get => _icon; }
        public bool unique { get => _unique; }
        public int range { get => _skillSelectionRange.range; }
        public bool ignoreTerrain { get => _skillSelectionRange.ignoreTerrain; }
        public bool includeSelf { get => _skillSelectionRange.includeSelf; }
        public Optional<Vector2[]> optionalRange { get => _skillSelectionRange.optionalRange; }
        public virtual bool castableOn(BattleBoardTile tile) => false;
        
        private void OnValidate()
        {
            if (_battleData == null)
                Debug.LogError($"Did not assign battle data for skill! {id} {this}");
        }
    }
    
    public enum SkillId
    {
        None,
        Punch,
        Move,
        Count,
    }
}