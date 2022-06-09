using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class UI_Menu<T> : MonoBehaviour where T : IUI_MenuItem<T>
    {
        [SerializeField] private RectTransform _itemContainer;
        [SerializeField] private List<GameObject> _items;
        [SerializeField] private GameObject _itemPrefab;
        
        public T AddItem(Action<T> callback = null)
        {
            GameObject newItemObject = Instantiate(_itemPrefab, _itemContainer);
            T newItem = newItemObject.GetComponent<T>();
            if (callback != null)
                newItem.confirmEvent += callback;
            _items.Add(newItemObject);
            return newItem;
        }

        public T GetItemAt(int i) => _items[i].GetComponent<T>();

        public void Clear()
        {
            foreach (GameObject item in _items)
            {
                Destroy(item);
            }
            
            _items.Clear();
        }
    }

    public interface IUI_MenuItem<T> where T : IUI_MenuItem<T>
    {
        public event Action<T> confirmEvent;
    }
}