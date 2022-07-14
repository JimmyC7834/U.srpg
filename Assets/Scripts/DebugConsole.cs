using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

namespace Game
{
    public class DebugConsole : MonoBehaviour
    {
        [SerializeField] private bool _showConsole = true;
        [SerializeField] private Dictionary<string, DebugItem> _debugItems;

        [Header("Console Settings")]
        private float _consoleWidth = Screen.width/3f;
        [SerializeField] private float _itemHeight;
        [SerializeField] private Vector2 _position;

        public void OnToggleDebug(InputValue inputValue)
        {
            _showConsole = !_showConsole;
        }

        public void Awake()
        {
            _debugItems = new Dictionary<string, DebugItem>();
        }

        public void AddItem(DebugItem _debugItem)
        {
            _debugItems.Add(_debugItem.name, _debugItem);
        }

        public void SetValue(string name, string value)
        {
            _debugItems[name] = DebugItem.From(name, value);
        }
        
        private void OnGUI()
        {
            if (!_showConsole) return;

            float y = 0f;
            foreach (DebugItem item in _debugItems.Values)
            {
                GUI.Box(new Rect(_position.x, _position.y + y, _consoleWidth, _itemHeight), item.message);
                y += _itemHeight;
            }
        }
    }
    
    [Serializable]
    public struct DebugItem
    {
        public string name { get; private set; }
        public string value { get; private set; }
        public Action<string> UpdateValue { get; private set; }
        public string message => $"{name}: {value}";

        public static DebugItem From(string _name, string _value) => new DebugItem()
        {
            name = _name,
            value = _value,
        };

        public void SetValue(string _value) => value = _value;
    }
}