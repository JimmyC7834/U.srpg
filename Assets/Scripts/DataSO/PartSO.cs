using System;
using Game.DataSet;
using Game.Unit.Ability;
using Game.Unit.Skill;
using UnityEngine;


namespace Game.Unit.Part
{
    [CreateAssetMenu(menuName = "Game/Unit/Part")]
    public class PartSO : DataEntrySO<PartId>
    {
        [SerializeField] private AbilityDataSetSO _abDataset;
        
        [SerializeField] private AbilityId[] _abilities;
        [SerializeField] private UnitStat.StatBoostEntry[] _statBoostEntries;
        private UnitStatModifier[] _statBoost;
        [SerializeField] private Sprite _icon;
        [SerializeField] private SkillId _skillId;

        public Sprite icon { get => _icon; }

        public UnitStatModifier[] statBoost
        {
            get
            {
                if (_statBoost == null)
                {
                    _statBoost = new UnitStatModifier[_statBoostEntries.Length];
                    for (int i = 0; i < _statBoostEntries.Length; i++)
                    {
                        _statBoost[i] = _statBoostEntries[i].ToModifier();
                    }
                }

                return _statBoost;
            }
        }
        public SkillId skillId { get => _skillId; }

        public AbilitySO[] GetAbilities()
        {
            AbilitySO[] abilities = new AbilitySO[_abilities.Length];
            for (int i = 0; i < _abilities.Length; i++)
                abilities[i] = _abDataset[_abilities[i]];
            return abilities;
        }
    }

    public enum PartId
    {
        None = -1,
        DebugPart = 10,
        RottenArm = 20,
        RottenTorso = 30,
        RottenLeg = 40,
        RottenHead = 50,
        SteelPlate = 60,
        Count = 6,
    }
}