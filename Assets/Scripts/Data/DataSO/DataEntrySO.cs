using System;
using UnityEngine;

namespace Game.DataSet
{
    public abstract class DataEntrySO<I> : ScriptableObject, IDataId<I> where I : Enum
    {
        [Header("Metadata")]
        [SerializeField] private string _displayName;
        [SerializeField] private I _id;
        [SerializeField] private Sprite _icon;
        
        public I ID { get => _id; }
        public Sprite icon { get => _icon; }
        public string displayName { get => (_displayName == "") ? _id.ToString() : _displayName; }
    }
}