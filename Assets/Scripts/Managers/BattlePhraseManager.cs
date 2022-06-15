using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Battle.Map;
using Game.Unit;
using Game.Unit.Skill;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

namespace Game.Battle
{
    public class BattlePhraseManager : MonoBehaviour
    {
        public class Phrase
        {
            private BattlePhraseManager _parent;
            private BattleData _battleData => _parent._battleData;
            private InputReader _input => _parent._input;
            private CursorController _cursor => _parent._cursor;
            
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
            
            public class UnitSelectionPhrase : Phrase
            {
                public UnitSelectionPhrase(BattlePhraseManager parent) : base(parent) { }

                public override void Enter()
                {
                    _cursor.OnConfirm += OnConfirm;
                    _input.EnableMapNaviInput();
                }

                public override void Exit()
                {
                    _cursor.OnConfirm -= OnConfirm;
                }

                private void OnConfirm(CursorController obj)
                {
                    if (_battleData.currentUnit == null) return;
                    Debug.Log($"Confrimed on unit {_battleData.currentUnit}");
                    _parent.Pop();
                    _parent.Push(new SkillSelectionPhrase(_parent));
                }
            }
            
            public class SkillSelectionPhrase : Phrase
            {
                public SkillSelectionPhrase(BattlePhraseManager parent) : base(parent) { }
                
                public override void Enter()
                {
                    _input.DisableAllInput();
                }

                public override void Start()
                {
                    _battleData.uiManager.OpenSkillSelectionMenu(
                        _battleData.currentUnit,
                        (item) =>
                        {
                            _battleData.SetCastedSkill(item.skill);
                            _parent.Pop();
                            _parent.Push(new TargetSelectionPhrase(_parent));
                        });
                    _input.EnableMenuNaviInput();
                }
            }
            
            public class TargetSelectionPhrase : Phrase
            {
                private BattleData.SelectionInfo _selectionInfo;
                
                public TargetSelectionPhrase(BattlePhraseManager parent) : base(parent) { }

                public override void Enter()
                {
                    _input.DisableAllInput();
                    UnitObject caster = _battleData.currentUnit;
                    SkillSO skill = _battleData.castedSkill;
                    Vector2Int casterLocation = caster.location;
                    _selectionInfo = GetRangeTilesFrom(
                        caster.location.x, caster.location.y, skill.range, skill.ignoreTerrain, skill.includeSelf, skill.optionalRange);

                    _battleData.mapHighlighter.HighlightTiles(
                        _selectionInfo.rangeTiles.Select(x => x.coord),
                        TileHighlightColor.InTargetRange);
                }

                public override void Start()
                {
                    _input.EnableMapNaviInput();
                    _cursor.OnConfirm += OnConfirm;
                }

                private void OnConfirm(CursorController cursor)
                {
                    if (!(_selectionInfo.rangeV2.Contains(cursor.MapCoord) && _battleData.castedSkill.castableOn(cursor.CurrentTile))) return;
                    _cursor.OnConfirm -= OnConfirm;
                    
                    _battleData.SetTargetTile(cursor.CurrentTile);
                    _battleData.mapHighlighter.RemoveHighlights();
                    _parent.Pop();
                    _parent.Push(new UnitSelectionPhrase(_parent));
                }

                private BattleData.SelectionInfo GetRangeTilesFrom(int gx, int gy, int totalRange, bool ignoreTerrain, bool includeSelf, Optional<Vector2[]> optionalRange)
                {
                    // BFS search
                    Queue<BattleBoardTile> queue = new Queue<BattleBoardTile>();
                    HashSet<Vector2> explored = new HashSet<Vector2>();
                    Dictionary<Vector2, float> tileCostLeft = new Dictionary<Vector2, float>();
                    Dictionary<Vector2, Vector2> tileParents = new Dictionary<Vector2, Vector2>();
                    HashSet<BattleBoardTile> rangeTiles = new HashSet<BattleBoardTile>();

                    BattleBoard battleBoard = _parent._battleData.battleBoard;
                    BattleBoardTile startTile = battleBoard.GetTile(gx, gy);
                    explored.Add(startTile.coord);
                    queue.Enqueue(startTile);
                    tileCostLeft.Add(startTile.coord, totalRange);

                    while (queue.Count != 0)
                    {
                        BattleBoardTile currentTile = queue.Dequeue();
                        rangeTiles.Add(currentTile);
                        foreach (Vector2 tileCoord in battleBoard.GetTile(currentTile.x, currentTile.y).neighbours)
                        {
                            BattleBoardTile tile = battleBoard.GetTile(tileCoord);
                            float costLeft = tileCostLeft[currentTile.coord] - (ignoreTerrain ? 1 : tile.cost);
                            if (!explored.Contains(tileCoord) && costLeft >= 0f && tile.walkable)
                            {
                                queue.Enqueue(tile);
                                explored.Add(tileCoord);

                                if (!tileParents.ContainsKey(tileCoord))
                                    tileParents.Add(tileCoord, currentTile.coord);

                                if (!tileCostLeft.ContainsKey(tileCoord))
                                    tileCostLeft.Add(tileCoord, costLeft);

                                tileCostLeft[tileCoord] = costLeft;
                            }
                        }
                    }

                    if (!includeSelf)
                        rangeTiles.Remove(startTile);

                    if (optionalRange.Enabled)
                    {
                        foreach (Vector2 v in optionalRange.Value)
                        {
                            Vector2 coord = v + _battleData.currentUnit.location;
                            if (battleBoard.CoordOnBoard(coord))
                                rangeTiles.Add(battleBoard.GetTile(coord));
                        }
                    }

                    return BattleData.SelectionInfo.From(rangeTiles, tileParents);
                }
            }
        }

        [SerializeField] private BattleData _battleData;
        [SerializeField] private InputReader _input;
        private Phrase _top => _stack.Peek();
        private CursorController _cursor => _battleData.cursor;
        private Stack<Phrase> _stack;
        private bool started = false;

        public void Update()
        {
            if (!started)
            {
                started = true;
                _top.Start();
            }
            
            _top.Update();
        }

        public void Clear() => _stack.Clear();
        public bool IsEmpty() => _stack.Count == 0;

        public void Initialize()
        {
            _stack = new Stack<Phrase>();
            // empty phrase to skip null check
            _stack.Push(new Phrase(this));
            
            Push(new Phrase.UnitSelectionPhrase(this));
        }
        
        // prev.Disable() -> new.Enter() -> new.Start()
        public void Push(Phrase state)
        {
            Debug.Log($"Pushed to Top: {state}");
            _top.Disable();
            _stack.Push(state);
            state.Enter();
            started = false;
        }
        
        // top.Disable() -> top.Exit() -> prev.Enable()
        public Phrase Pop()
        {
            _top.Disable();
            _top.Exit();
            Phrase poped = _stack.Pop();
            _top.Enable();
            Debug.Log($"Popped: {poped}");
            return poped;
        }
    }
}