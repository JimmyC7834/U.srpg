using System;
using System.Collections.Generic;
using Game.Unit.Part;
using Game.Unit.Skill;

namespace Game.Unit
{
    [Serializable]
    public struct PartNode
    {
        public PartSO part;
        public Optional<PartNode[]> children;

        public PartNode Copy()
        {
            PartNode newNode = new PartNode();
            newNode.part = part;
            if (children.Enabled)
            {
                newNode.children = new Optional<PartNode[]>(new PartNode[children.Value.Length]);
                for (int i = 0; i < children.Value.Length; i++)
                {
                    newNode.children.Value[i] = children.Value[i].Copy();
                }
            }

            return newNode;
        }
    }
    
    /**
     * Immutable Tree represent the parts that forms the unit
     */
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
                if (partNode.children.Enabled)
                {
                    Children = new UnitPartTreeNode[partNode.children.Value.Length];
                    for (int i = 0; i < partNode.children.Value.Length; i++)
                    {
                        Children[i] = new UnitPartTreeNode(nodes, partNode.children.Value[i]);
                    }
                }
            }
        }

        private readonly UnitObject _unit;
        public readonly UnitPartTreeNode root;
        private readonly List<UnitPartTreeNode> _nodes;
        
        public UnitPartTree(UnitObject _unit, PartNode partTree)
        {
            this._unit = _unit;
            _nodes = new List<UnitPartTreeNode>();
            root = new UnitPartTreeNode(_nodes, partTree);
        }

        public List<SkillSO> GetAllSkills()
        {
            List<SkillSO> skills = new List<SkillSO>();
            // foreach (UnitPartTreeNode node in _nodes)
            // {
            //     if (node.Part.skillId == SkillId.None)
            //         continue;
            //     
            //     SkillSO skill = _unit._skillDataSet[node.Part.skillId];
            //     if (!(skill.unique && skills.Contains(skill)))
            //         skills.Add(skill);
            // }

            return skills;
        }
    }
}