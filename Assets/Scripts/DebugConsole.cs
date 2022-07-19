using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
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

        public void AddItem(string name, Func<string> bind)
        {
            DebugItem _debugItem = DebugItem.From(name, bind);
            if (!_debugItems.ContainsKey(name))
                _debugItems.Add(_debugItem.name, _debugItem);
            RebindValue(name, bind);
        }

        public void RebindValue(string name, Func<string> bind)
        {
            _debugItems[name] = DebugItem.From(name, bind);
        }
        
        private void OnGUI()
        {
            if (!_showConsole) return;

            float y = _itemHeight;
            GUI.Box(new Rect(_position.x, _position.y, _consoleWidth, _itemHeight * (_debugItems.Count + 1)), "");
            foreach (DebugItem item in _debugItems.Values)
            {
                GUI.Label(new Rect(_position.x + 10, _position.y + y, _consoleWidth, y), item.message);
                y += _itemHeight;
            }
        }
    }
    
    [Serializable]
    public struct DebugItem
    {
        public string name { get; private set; }
        public Func<string> bind { get; private set; }
        public string message => $"{name}: {bind.Invoke()}";

        public static DebugItem From(string _name, Func<string> _bind) => new DebugItem()
        {
            name = _name,
            bind = _bind,
        };
    }
}