using Game.DataSet;
using Game.Unit.Ability;
using Game.Unit.Skill;
using UnityEngine;

namespace Game.Unit.Part
{
    [CreateAssetMenu(menuName = "Game/DataEntry/Part")]
    public class PartSO : DataEntrySO<PartId>
    {
        [SerializeField] private AbilityDataSetSO _abDataset;
        
        [SerializeField] private AbilityID[] _abilities;
        [SerializeField] private UnitParam.ParamBoostEntry[] _paramBoostEntries;
        private UnitParamModifier[] _paramBoosts;
        [SerializeField] private SkillId _skillId;
        
        public UnitParamModifier[] ParamBoosts
        {
            get
            {
                if (_paramBoosts == null)
                {
                    _paramBoosts = new UnitParamModifier[_paramBoostEntries.Length];
                    for (int i = 0; i < _paramBoostEntries.Length; i++)
                    {
                        _paramBoosts[i] = _paramBoostEntries[i].ToModifier();
                    }
                }

                return _paramBoosts;
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