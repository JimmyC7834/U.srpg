using System;
using System.Collections;
using System.Collections.Generic;
using Game.Battle;
using Game.Battle.Map;
using Game.DataSet;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game.Unit.Skill
{
    public abstract class SkillSO : DataEntrySO<SkillId>
    {
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
        
        public abstract bool castableOn(BattleBoardTile tile);

        public virtual void StartCasting(
            BattleService battleService, SkillCastInfo skillCastInfo, SkillCaster.SelectionInfo selectionInfo, Action callback)
        {
            UnitObject casterObject = skillCastInfo.casterTile.unitOnTile;
            casterObject.StartCoroutine(CallBackWrap(Cast(battleService, skillCastInfo, selectionInfo), callback));
        }

        public IEnumerator CallBackWrap(IEnumerator enumerator, Action callback)
        {
            yield return enumerator;
            Assert.AreNotEqual(callback, null);
            callback.Invoke();
        }

        public abstract IEnumerator Cast(BattleService battleService, SkillCastInfo skillCastInfo, SkillCaster.SelectionInfo selectionInfo);
    }
    
    public enum SkillId
    {
        None,
        Punch,
        Move,
        Count,
    }
}