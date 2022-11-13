using System;
using UnityEngine;

namespace Game.UI
{
    public abstract class UI_View : MonoBehaviour
    {
        // public Action OnEnter = delegate {  };
        // public Action OnExit = delegate {  };

        public virtual void Enter() { }

        public virtual void Exit() { }

        // public void Enter() => OnEnter.Invoke();

        // public void Exit() => OnExit.Invoke();
    }
}