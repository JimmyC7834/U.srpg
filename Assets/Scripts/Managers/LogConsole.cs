using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class LogConsole : MonoBehaviour
    {
        [SerializeField] private bool _showConsole = true;
        private List<LogItem> _logItems;
        
        [Header("Item Settings")]
        private float _consoleWidth = Screen.width;
        [SerializeField] private float _itemHeight;
        [SerializeField] private Vector2 _position;
        [SerializeField] private float _duration; 

        public void Awake()
        {
            _logItems = new List<LogItem>();
        }

        public void SendText(string _text) => SendText(_text, Color.white);
        public void SendText(string _text, Color _color)
        {
            LogItem newItem = LogItem.From(_text, _color);
            _logItems.Add(newItem);
            StartCoroutine(CountDownRemoveLog(newItem));
        }

        private IEnumerator CountDownRemoveLog(LogItem item)
        {
            yield return new WaitForSecondsRealtime(_duration);
            _logItems.Remove(item);
        }

        private void OnGUI()
        {
            if (!_showConsole) return;

            float y = _itemHeight;
            for (int i = _logItems.Count - 1; i >= 0; i--)
            {
                GUI.Label(new Rect(_position.x + 10, _position.y - y, _consoleWidth, y), _logItems[i].text);
                y += _itemHeight;
            }
        }
        
        private struct LogItem
        {
            public Color color { get; private set; }
            public string text { get; private set; }

            public static LogItem From(string _text, Color _color) => new LogItem()
            {
                color = _color,
                text = _text,
            };
        }
    }
}
