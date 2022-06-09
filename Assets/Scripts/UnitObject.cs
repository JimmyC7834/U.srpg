using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.DataSet;
using Game.Unit.Ability;
using Game.Unit.Part;
using Game.Unit.Skill;
using UnityEngine;

namespace Game.Unit
{
    public class UnitObject : MonoBehaviour
    {
        [SerializeField] private SkillDataSetSO _skillDataSet;
            
        private UnitSO unitSO;
        
        public string displayName;
        public Sprite sprite;
        public DataSet.UnitId unitId;
        public UnitParam unitParam;
        public UnitPartTree partTree;
        
        public void InitializeWith(UnitSO unitSO)
        {
            this.unitSO = unitSO;

            unitParam = gameObject.AddComponent<UnitParam>().Initialize(unitSO);
            
            partTree = new UnitPartTree(this, unitSO.PartTree);
            RegisterParts(partTree.root);
        }

        private void RegisterParts(UnitPartTree.UnitPartTreeNode node)
        {
            RegisterPart(node);
            for (int i = 0; i < node.Children.Length; i++)
                RegisterParts(node.Children[i]);
        }
        
        private void RegisterPart(UnitPartTree.UnitPartTreeNode node)
        {
            if (node.Part == null)
            {
                Debug.LogWarning($"{"null part data!!"}");
                return;
            }
            
            AbilitySO[] abilities = node.Part.GetAbilities();
            for (int i = 0; i < abilities.Length; i++)
                abilities[i].RegisterTo(this, node);
        }
        
        public class UnitPartTree
        {
            public class UnitPartTreeNode
            {
                public PartSO Part { get; }
                public UnitPartTreeNode[] Children { get; }
    
                public UnitPartTreeNode(List<UnitPartTreeNode> nodes, PartNode partNode)
                {
                    nodes.Add(this);
                    Part = partNode.part;
                    Children = new UnitPartTreeNode[partNode.children.Length];
                    for (int i = 0; i < partNode.children.Length; i++)
                    {
                        Children[i] = new UnitPartTreeNode(nodes, partNode.children[i]);
                    }
                }
            }

            public UnitObject unitObject;
            public UnitPartTreeNode root;
            public List<UnitPartTreeNode> nodes;
            
            public UnitPartTree(UnitObject _unitObject, PartNode partTree)
            {
                unitObject = _unitObject;
                nodes = new List<UnitPartTreeNode>();
                root = new UnitPartTreeNode(nodes, partTree);
            }
    
            public List<SkillId> GetAllSkillIds()
            {
                List<SkillSO> skills = GetAllSkills();
                List<SkillId> skillIds = new List<SkillId>();
                foreach (SkillSO skill in skills)
                {
                    skillIds.Add(skill.id);
                }
            
                return skillIds;
            }
            
            public List<SkillSO> GetAllSkills()
            {
                List<SkillSO> skills = new List<SkillSO>();
                foreach (UnitPartTreeNode node in nodes)
                {
                    if (node.Part.skillId == SkillId.None)
                        continue;
                    
                    SkillSO skill = unitObject._skillDataSet[node.Part.skillId];
                    if (!(skill.unique && skills.Contains(skill)))
                        skills.Add(skill);
                }
    
                return skills;
            }
        }
    }
}