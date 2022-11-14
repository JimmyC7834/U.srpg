using System;
using UnityEngine;

namespace Game.UI
{
    public class UI_View : MonoBehaviour
    {
        public Action OnEnter = delegate {  };
        public Action OnExit = delegate {  };

        protected virtual void Enter() { }

        protected virtual void Exit() { }

        public void EnterView()
        {
            Enter();            
            OnEnter.Invoke();
        }

        public void ExitView()
        {
            Exit();
            OnExit.Invoke();
        }
    }
}