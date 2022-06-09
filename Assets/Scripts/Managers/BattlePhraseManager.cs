using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Game.Battle
{
    public class BattlePhraseManager : MonoBehaviour
    {
        // private enum BattlePhrase
        // {
        //     UnitSelection,
        //     SkillSelection,
        //     TargetSelection,
        //     Count,
        // }

        public class Phrase
        {
            private BattlePhraseManager _parent;
            
            public virtual void Enter() { }
            public virtual void Start() { }
            public virtual void Update() { }
            public virtual void Exit() { }

            public Phrase(BattlePhraseManager bm)
            {
                _parent = bm;
            }
            
            public class UnitSelectionPhrase : Phrase
            {
                public UnitSelectionPhrase(BattlePhraseManager parent) : base(parent) { }

                public override void Enter()
                {
                    _parent.cursor.OnConfirm += OnConfirm;
                    _parent._input.EnableMapNaviInput();
                }

                public override void Exit()
                {
                    _parent.cursor.OnConfirm -= OnConfirm;
                }

                private void OnConfirm(CursorController obj)
                {
                    if (_parent._battleData.currentUnit == null) return;
                    Debug.Log($"Confrimed on unit {_parent._battleData.currentUnit}");
                    _parent.Pop();
                    _parent.Push(new SkillSelectionPhrase(_parent));
                }
            }
            
            public class SkillSelectionPhrase : Phrase
            {
                public SkillSelectionPhrase(BattlePhraseManager parent) : base(parent) { }
                public bool init = false;
                
                public override void Enter()
                {
                    _parent._input.DisableAllInput();
                }

                public override void Update()
                {
                    if (!init)
                    {
                        _parent._battleData.uiManager.OpenSkillSelectionMenu(
                            _parent._battleData.currentUnit,
                            (item) =>
                            {
                                _parent._battleData.SetCastedSkill(item.skill);
                                _parent.Pop();
                                _parent.Push(new TargetSelectionPhrase(_parent));
                            });
                        _parent._input.EnableMenuNaviInput();
                        init = true;
                    }
                }
            }
            
            public class TargetSelectionPhrase : Phrase
            {
                public TargetSelectionPhrase(BattlePhraseManager parent) : base(parent) { }

                public override void Enter()
                {
                    _parent._input.EnableMapNaviInput();
                }

                private void OnConfirm(UI.UI_SkillSelectionMenuItem item)
                {
                    if (_parent._battleData.currentUnit == null) return;
                    
                }
            }
        }

        [SerializeField] private BattleData _battleData;
        [SerializeField] private InputReader _input;
        private Stack<Phrase> _stack;
        
        public void Update() => _stack.Peek().Update();

        public void Clear() => _stack.Clear();

        public Phrase Peek() => _stack.Peek();

        public bool IsEmpty() => _stack.Count == 0;
        
        private CursorController cursor => _battleData.cursor;

        public void Initialize()
        {
            _stack = new Stack<Phrase>();
            Push(new Phrase.UnitSelectionPhrase(this));
        }
        
        
        public void Push(Phrase state)
        {
            Debug.Log("Pushed to Top: " + state);
            // if (_stack.Count > 0) _stack.Peek().Disable();
            _stack.Push(state);
            state.Enter();
        }

        public Phrase Pop()
        {
            _stack.Peek().Exit();
            Phrase top = _stack.Pop();
            // _stack.Peek().Enable();
            Debug.Log("Popped: " + top);
            return top;
        }
    }
}