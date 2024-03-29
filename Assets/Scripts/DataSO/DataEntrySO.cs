using System;
using UnityEngine;

namespace Game.DataSet
{
    public class DataEntrySO<I> : ScriptableObject, IDataId<I> where I : Enum
    {
        [SerializeField] private string _displayName;
        [SerializeField] private I _id;
        [SerializeField] private Sprite _icon;
        
        public I id { get => _id; }
        public Sprite icon { get => _icon; }
        public string displayName { get => (_displayName == "") ? _id.ToString() : _displayName; }
    }
}