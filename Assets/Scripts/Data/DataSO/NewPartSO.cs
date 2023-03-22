using System;
using Game.Unit.Ability;
using Game.Unit.Skill;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPartSO", menuName = "Game/SO/NewPart")]
public class NewPartSO : ScriptableObject
{
    [SerializeField] private Sprite _icon;

    // [Header("Display Information")]
    [SerializeField] private string _displayName;
    [TextArea]
    [SerializeField] private string _description;

    [Space] 
    
    // [Header("Skill and Abilities")]
    [SerializeField] private SkillId _skill;
    [SerializeField] private AbilityContainer[] _abilities;
    [SerializeField] private int _abilityActivation;

    // [Space] [Header("Matching Setting")]
    [SerializeField] private bool _compatUp;
    [SerializeField] private bool _compatDown;
    [SerializeField] private bool _compatLeft;
    [SerializeField] private bool _compatRight;
    [SerializeField] private bool[] _compatibility;

    public Sprite icon { get => _icon; }
    // public string displayName { get => _displayName; }
    // public string description { get => _description; }
    // public SkillId skill { get => _skill; }
    // public AbilityContainer[] abilities { get => _abilities; }
    public bool[] compatibility { get => _compatibility; }

    [Serializable]
    public struct AbilityContainer
    {
        public AbilityID abilityId;
        public bool locked;
    }
}
