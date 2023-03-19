using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Game.DataSet;
using TMPro;

namespace Game.UI
{
    /**
     * Base class for menu for data entries
     */
    // TODO: remake menu logic
    public abstract class UI_DataEntryMenu<T, E> : MonoBehaviour where T : DataEntrySO<E> where E : Enum
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

        public void OpenMenu(List<T> dataEntries, Action<T> _callback)
        {
            gameObject.SetActive(true);
            callback = _callback;
            foreach (T entry in dataEntries)
            {
                AddItem(entry);
            }
        }
        
        public UI_DataEntryMenuItem<T, E> AddItem(T dataEntry)
        {
            UI_DataEntryMenuItem<T, E> newItem = 
                _pool.Get((item) => item.Initialize(dataEntry, OnConfirmed));
            items.Add(newItem);
            return newItem;
        }

        public int Count() => items.Count;

        public UI_DataEntryMenuItem<T, E> GetItemObject(int i)
        {
            if (i >= items.Count)
                throw new IndexOutOfRangeException();

            return items[i];
        }
        
        private void OnConfirmed(T dataEntry)
        {
            callback.Invoke(dataEntry);
            CloseMenu();
        }
        
        public void CloseMenu()
        {
            for (int i = 0; i < items.Count; i++)
            {
                // items[i].transform.SetParent(_transform);
                _pool.Release(items[i]);
            }
            
            callback = delegate {  };
            items.Clear();
        }
    }
    
    /**
     * Immutable Object of menu item.
     * Initialize should only be called by corresponding UI_DataEntryMenu as constructor.
     */
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