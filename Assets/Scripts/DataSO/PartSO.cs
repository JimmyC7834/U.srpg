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
        
        private Sprite _icon;
        private UnitStatModifier[] _stateBoost;
        private SkillId _skillId;

        public Sprite icon { get => _icon; }
        public UnitStat stateBoost { get => stateBoost; }
        public SkillId skillId { get => skillId; }

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
        RottenArm,
        RottenTorso,
        RottenLeg,
        RottenHead,
        Count,
    }
}