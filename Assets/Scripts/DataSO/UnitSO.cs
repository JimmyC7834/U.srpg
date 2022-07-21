using System;
using System.Collections;
using System.Collections.Generic;
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

    [CreateAssetMenu(menuName = "Game/Unit/UnitSO")]
    public class UnitSO : DataEntrySO<UnitId>
    {
        [SerializeField] private Sprite _sprite;
        [SerializeField] private AnimatorOverrideController _animatorOverrideController;
        [SerializeField] private PartNode _partTree;

        // make a copy of the raw data everytime to prevent change on dataset
        public PartNode PartTree => _partTree.Copy();
        public Sprite sprite { get => _sprite; }
        public AnimatorOverrideController animatorOverrideController { get => _animatorOverrideController; }
    }

    public enum UnitId
    {
        None,
        DebugUnit,
        Norm,
        Norm1,
        Norm2,
        Norm3,
        Count
    }
}

