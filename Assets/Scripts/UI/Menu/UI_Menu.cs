using System;
using System.Collections.Generic;
using Game.DataSet;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    // TODO: remake menu logic
    public abstract class UI_DataEntryMenu<T, E> : UI_View where T : DataEntrySO<E> where E : Enum
    {
        [SerializeField] private RectTransform _itemContainer;
        [SerializeField] private UI_DataEntryMenuItem<T, E> _prefab;
        private GameObjectPool<UI_DataEntryMenuItem<T, E>> _pool;
        protected List<UI_DataEntryMenuItem<T, E>> items { get; set; }
        protected event Action<T> callback = delegate {  };
        private Transform _transform;

        public void Awake()
        {
            items = new List<UI_DataEntryMenuItem<T, E>>();
            _transform = transform;
            _pool = new GameObjectPool<UI_DataEntryMenuItem<T, E>>(_prefab, _itemContainer);
        }

        public UI_DataEntryMenuItem<T, E> AddItem(T dataEntry)
        {
            UI_DataEntryMenuItem<T, E> newItem = 
                _pool.Get((item) => item.Initialize(dataEntry, OnConfirmed_));
            items.Add(newItem);
            return newItem;
        }

        private void OnConfirmed_(T dataEntry)
        {
            callback.Invoke(dataEntry);
            OnConfirmed(dataEntry);
        }
        
        public abstract void OnConfirmed(T dataEntry);
        
        public void Clear()
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i].transform.SetParent(_transform);
                _pool.Release(items[i]);
            }
            
            callback = delegate {  };
            items.Clear();
        }
    }
    
    public class UI_DataEntryMenuItem<T, E> : MonoBehaviour where T : DataEntrySO<E> where E : Enum
    {
        public T _dataEntry;
        public Image iconImg;
        public Button button;
        public TMP_Text itemText;
        
        public event Action<T> onConfirm = delegate {  };
        
        public virtual void Initialize(T dataEntry, Action<T> callback)
        {
            _dataEntry = dataEntry;
            if (callback == null) callback = delegate {  };
            onConfirm = callback;
            iconImg.sprite = _dataEntry.icon;
            itemText.SetText(dataEntry.name);

            button.onClick.AddListener(Confirm);
        }

        public void Confirm() => onConfirm.Invoke(_dataEntry);
    }
}