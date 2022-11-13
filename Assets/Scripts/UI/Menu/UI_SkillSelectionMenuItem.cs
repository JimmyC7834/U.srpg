using System;
using Game.Unit.Skill;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class UI_SkillSelectionMenuItem : UI_DataEntryMenuItem<SkillSO, SkillId>
    {
        [SerializeField] private TMP_Text _costText;

        public override void Initialize(SkillSO dataEntry, Action<SkillSO> callback)
        {
            base.Initialize(dataEntry, callback);
            _costText.SetText(dataEntry.cost.ToString());
        }
    }
}