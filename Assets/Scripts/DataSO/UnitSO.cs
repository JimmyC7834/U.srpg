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
        public PartNode[] children;

        public PartNode Copy()
        {
            PartNode newNode = new PartNode();
            newNode.part = part;
            newNode.children = new PartNode[children.Length];
            for (int i = 0; i < children.Length; i++)
            {
                newNode.children[i] = children[i].Copy();
            }

            return newNode;
        }
    }

    [CreateAssetMenu(menuName = "Game/Unit/UnitSO")]
    public class UnitSO : DataEntrySO<UnitId>
    {
        public Sprite sprite;

        [SerializeField] private PartNode _partTree;

        // make a copy of the raw data everytime to prevent change on dataset
        public PartNode PartTree => _partTree.Copy();
    }

    public enum UnitStatType
    {
        DUR,
        STR,
        DEX,
        PER,
        SAN,
        Count,
    }
}

