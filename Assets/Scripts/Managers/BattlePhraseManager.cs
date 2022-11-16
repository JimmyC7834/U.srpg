using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Battle
{
    public enum BattleTeam
    {
        Player,
        CPU,
    }
    
    /**
     * Provide an interface to manage a stack of BattlePhrases for
     * switching current game logic.
     */
    public class BattlePhraseManager : MonoBehaviour
    {

        [SerializeField] private InputReader _input;
        private BattleService _battleService;
        public InputReader input { get => _input; }
        public BattleService battleService { get => _battleService; }
        
        private Phrase _top => _stack.Peek();
        private Stack<Phrase> _stack;
        private bool started = false;

        public event Action<Phrase> OnPhraseChanged;

        public void Start()
        {
            _battleService.debugConsole.AddItem("Current Phrase", () => _top.ToString());
        }

        public void Update()
        {
            if (!started)
            {
                started = true;
                _top.Start();
            }
            
            _top.Update();
        }
        
        public void Initialize(BattleService battleService)
        {
            _battleService = battleService;

            // empty phrase to skip null check
            _stack = new Stack<Phrase>();
            _stack.Push(new Phrase(this));
            
            Push(new HandleKokuPhrase(this));
        }
        
        // prev.Disable() -> new.Enter() -> new.Start()
        public void Push(Phrase state)
        {
            _top.Disable();
            _stack.Push(state);
            state.Enter();
            started = false;
            OnPhraseChanged?.Invoke(_top);
        }
        
        // top.Disable() -> top.Exit() -> prev.Enable()
        public Phrase Pop()
        {
            _top.Disable();
            _top.Exit();
            Phrase poped = _stack.Pop();
            _top.Enable();
            OnPhraseChanged?.Invoke(_top);
            return poped;
        }
    }
}