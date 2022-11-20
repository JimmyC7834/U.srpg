namespace Game.Battle
{
    /**
     * A phrase representing the current game logic/control
     */
    public class Phrase
    {
        protected BattlePhraseManager _parent;
        protected BattleService battleService => _parent.battleService;
        protected InputReader _input => _parent.input;
        protected CursorController _cursor => _parent.battleService.cursor;
            
        public virtual void Enter() { }
        public virtual void Start() { }
        public virtual void Enable() { }
        public virtual void Disable() { }
        public virtual void Update() { }
        public virtual void Exit() { }

        public Phrase(BattlePhraseManager bm)
        {
            _parent = bm;
        }
    }

}