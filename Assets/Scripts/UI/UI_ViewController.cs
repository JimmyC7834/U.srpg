using System.Collections.Generic;
using UnityEngine.Assertions;

namespace Game.UI
{
    public class UI_ViewController
    {
        private struct ViewItem
        {
            public UI_View view { get; private set;  }
            public bool ExitOnNextPop { get; private set; }
            public bool ExitOnNextPush { get; private set; }

            public static ViewItem From(UI_View _view) => new ViewItem
            {
                view = _view,
                ExitOnNextPop = false,
                ExitOnNextPush = false,
            };
            
            public static ViewItem FromExitOnNextPop(UI_View _view) => new ViewItem
            {
                view = _view,
                ExitOnNextPop = true,
                ExitOnNextPush = false,
            };
            
            public static ViewItem FromExitOnNextPush(UI_View _view) => new ViewItem
            {
                view = _view,
                ExitOnNextPop = false,
                ExitOnNextPush = true,
            };
        }
        
        private Stack<ViewItem> _stack;

        public UI_ViewController()
        {
            _stack = new Stack<ViewItem>();
        }

        private void EnterView(UI_View view)
        {
            // pop current top is it's exit on next push
            if (_stack.Count != 0 && _stack.Peek().ExitOnNextPush)
                PopView();
            
            view.EnterView();
        }
        
        public void PushView(UI_View view)
        {
            EnterView(view);
            _stack.Push(ViewItem.From(view));
        }
        
        public void PushExitOnNextPopView(UI_View view)
        {
            EnterView(view);
            _stack.Push(ViewItem.FromExitOnNextPop(view));
        }
        
        public void PushExitOnNextPushView(UI_View view)
        {
            EnterView(view);
            _stack.Push(ViewItem.FromExitOnNextPush(view));
        }

        public UI_View PopView()
        {
            Assert.IsTrue(_stack.Count != 0);
            _stack.Peek().view.ExitView();
            
            // return directly if stack is empty or next view doesn't exit on next pop
            if (_stack.Count == 1 || !_stack.Peek().ExitOnNextPop)
                return _stack.Pop().view;

            UI_View top = _stack.Pop().view;
            _stack.Peek().view.ExitView();
            _stack.Pop();
            return top;
        }
    }
}