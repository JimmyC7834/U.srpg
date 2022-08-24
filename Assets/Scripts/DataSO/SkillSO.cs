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
    public interface ISk_Move { }
    public interface ISk_Attack { }
    public interface ISk_Heal { }
    public interface ISk_Buff { }
    public interface ISk_Debuff { }

    public abstract class SkillSO : DataEntrySO<SkillId>
    {
        [SerializeField] private bool _unique;
        [SerializeField] private int _cost;
        [SerializeField] private List<SkillTypeTag> _skillTypeTags;

        [Serializable]
        private struct SkillSelectionRange
        {
            public int range;
            public bool calWithMoveRange;
            public bool ignoreTerrain;
            public bool includeSelf;
            public Optional<Vector2[]> optionalRange;
        }

        [SerializeField] private SkillSelectionRange _skillSelectionRange;
        
        public bool unique { get => _unique; }
        public int range { get => _skillSelectionRange.range; }
        public bool calWithMoveRange { get => _skillSelectionRange.calWithMoveRange; }
        public bool ignoreTerrain { get => _skillSelectionRange.ignoreTerrain; }
        public bool includeSelf { get => _skillSelectionRange.includeSelf; }
        public Optional<Vector2[]> optionalRange { get => _skillSelectionRange.optionalRange; }
        
        public abstract bool castableOn(BattleBoardTile tile);

        public virtual void StartCasting(
            BattleService battleService, SkillCastInfo skillCastInfo, SkillCaster.SelectionInfo selectionInfo, Action callback)
        {
            UnitObject casterObject = skillCastInfo.casterTile.unitOnTile;
            casterObject.param.ConsumeAP(_cost);
            if (skillCastInfo.target == null)
                battleService.logConsole.SendText($"{casterObject.displayName} casted {skillCastInfo.castedSkill.displayName}");
            else
                battleService.logConsole.SendText($"{casterObject.displayName} casted {skillCastInfo.castedSkill.displayName} to {skillCastInfo.target.displayName}");

            casterObject.StartCoroutine(CallBackWrap(Cast(battleService, skillCastInfo, selectionInfo), callback));
        }

        public IEnumerator CallBackWrap(IEnumerator enumerator, Action callback)
        {
            yield return enumerator;
            Assert.AreNotEqual(callback, null);
            callback.Invoke();
        }

        public abstract IEnumerator Cast(BattleService battleService, SkillCastInfo skillCastInfo, SkillCaster.SelectionInfo selectionInfo);

        public bool IsTagged(SkillTypeTag tag) => _skillTypeTags.Contains(tag);
    }
    
    public enum SkillId
    {
        None = -1,
        Punch = 9910,
        EmptyPunch = 9920,
        Move = 9930,
        DealPoison1 = 9940,
        OneWayEnter = 9950,
        Count = 4,
    }
}