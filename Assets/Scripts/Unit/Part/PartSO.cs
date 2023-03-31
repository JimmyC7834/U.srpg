using Game.DataSet;
using Game.Unit.Ability;
using Game.Unit.Skill;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

namespace Game.Unit.Part
{
    [CreateAssetMenu(menuName = "Game/DataEntry/Part")]
    public class PartSO : DataEntrySO<PartID>
    {
        [SerializeField] private AbilityDataSetSO _abDataset;
        [SerializeField] private SkillDataSetSO _skDataset;

        [SerializeField] private PartID _id;
        
        [SerializeField] private UnitParam.ParamBoostEntry[] _paramBoostEntries;
        [SerializeField] private AbilityID[] _abilityIDs;
        [SerializeField] private SkillID _skillID;

        public SkillID skillID { get => _skillID; }
        public PartID ID { get => _id; }

        public UnitPart Create()
        {
            UnitAbility[] abilityRegs = new UnitAbility[_abilityIDs.Length];
            for (int i = 0; i < _abilityIDs.Length; i++)
                abilityRegs[i] = _abDataset[_abilityIDs[i]].Create(1);
            return new UnitPart(_id, abilityRegs, _skillID, _paramBoostEntries);;
        }
    }

    // TODO: Revise Skill code
    public class UnitPart
    {
        public readonly PartID ID;
        public readonly ReadOnlyArray<UnitAbility> Abilities;
        private readonly AbilityID[] _abilityIDs;
        private readonly SkillID _skillID;
        private readonly ReadOnlyArray<UnitParam.ParamBoostEntry> _paramBoostEntries;

        public UnitPart(PartID partID, UnitAbility[] _abilities, SkillID skillID,
            UnitParam.ParamBoostEntry[] paramBoostEntries)
        {
            ID = partID;
            Abilities = new ReadOnlyArray<UnitAbility>(_abilities);
            _skillID = skillID;
            _paramBoostEntries = new ReadOnlyArray<UnitParam.ParamBoostEntry>(paramBoostEntries);
        }
        
        public UnitParamModifier[] GetParamModifiers()
        {
            UnitParamModifier[] modifiers = new UnitParamModifier[_paramBoostEntries.Count];
            for (int i = 0; i < _paramBoostEntries.Count; i++)
                modifiers[i] = _paramBoostEntries[i].ToModifier(this);
            return modifiers;
        }
    }

    public enum PartID
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