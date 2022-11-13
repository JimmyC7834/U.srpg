using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class UI_ViewController
    {
        private Stack<UI_View> _stack;

        public UI_ViewController()
        {
            _stack = new Stack<UI_View>();
        }

        public void PushView(UI_View view)
        {
            view.Enter();
            _stack.Push(view);
        }
        
        public UI_View PopView()
        {
            _stack.Peek().Exit();
            return _stack.Pop();
        }
    }
}