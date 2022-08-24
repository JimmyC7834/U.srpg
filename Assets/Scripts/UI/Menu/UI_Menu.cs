using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

namespace Game.UI
{
    public class UI_Menu<T> : MonoBehaviour where T : MonoBehaviour, IUI_MenuItem<T>
    {
        [SerializeField] private RectTransform _itemContainer;
        [SerializeField] private GameObject _itemPrefab;
        [SerializeField] private List<T> _items;
        private ObjectPool<T> _pool;

        public void Awake()
        {
            _items = new List<T>();
            _pool = new ObjectPool<T>(
                CreatItem,
                PoolItem,
                ReleaseItem
            );
        }

        private T CreatItem() => Instantiate(_itemPrefab, _itemContainer).GetComponent<T>();
        
        private void PoolItem(T item)
        {
            item.gameObject.SetActive(true);
        }
        
        private void ReleaseItem(T item)
        {
            item.gameObject.SetActive(false);
        }

        public T AddItem(Action<T> callback = null)
        {
            T newItem = _pool.Get();
            if (callback != null)
                newItem.confirmEvent += callback;
            _items.Add(newItem);
            return newItem;
        }

        public T GetItemAt(int i) => _items[i].GetComponent<T>();

        public void Clear()
        {
            foreach (T item in _items)
            {
                _pool.Release(item);
            }
            
            _items.Clear();
        }
    }

    public interface IUI_MenuItem<T> where T : IUI_MenuItem<T>
    {
        public event Action<T> confirmEvent;
    }
}