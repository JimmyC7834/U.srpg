using System;
using System.Collections;
using System.Collections.Generic;
using Game.Battle;
using Game.DataSet;
using UnityEngine;

namespace Game.Unit
{
    [Serializable]
    public struct PartNode
    {
        public Part.PartSO part;
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

    [CreateAssetMenu(menuName = "Game/DataEntry/Unit")]
    public class UnitSO : DataEntrySO<UnitId>
    {
        [SerializeField] private Sprite _sprite;
        [SerializeField] private CpuUnitAI _ai;
        [SerializeField] private AnimatorOverrideController _animatorOverrideController;
        [SerializeField] private PartNode _partTree;

        // make a copy of the raw data everytime to prevent change on dataset
        public PartNode PartTree => _partTree.Copy();
        public Sprite sprite { get => _sprite; }
        public CpuUnitAI ai { get => _ai; }
        public AnimatorOverrideController animatorOverrideController { get => _animatorOverrideController; }
    }

    public enum UnitId
    {
        None = -1,
        PlayerUnit1 = 1000,
        DebugUnit = 9900,
        Norm = 9910,
        Norm1 = 9920,
        Norm2 = 9930,
        Norm3 = 9940,
        Count = 5
    }
}

