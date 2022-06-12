using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Battle.Map;
using Game.Unit;
using Game.Unit.Skill;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Game.Battle
{
    public class BattlePhraseManager : MonoBehaviour
    {
        public class Phrase
        {
            private BattlePhraseManager _parent;
            
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
                    _parent._cursor.OnConfirm += OnConfirm;
                    _parent._input.EnableMapNaviInput();
                }

                public override void Exit()
                {
                    _parent._cursor.OnConfirm -= OnConfirm;
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
                
                public override void Enter()
                {
                    _parent._input.DisableAllInput();
                }

                public override void Start()
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
                }
            }
            
            public class TargetSelectionPhrase : Phrase
            {
                private SelectionInfo _selectionInfo;
                
                public TargetSelectionPhrase(BattlePhraseManager parent) : base(parent) { }
                private struct RangeTile
                {
                    public int x, y;
                    public Vector2 parentCoord;
                }
                
                public override void Enter()
                {
                    _parent._input.DisableAllInput();
                    UnitObject caster = _parent._battleData.currentUnit;
                    SkillSO skill = _parent._battleData.castedSkill;
                    Vector2Int casterLocation = caster.location;
                    _selectionInfo = GetRangeTilesFrom(
                        caster.location.x, caster.location.y, skill.Range(), true, true);

                    _parent._battleData._mapHighlighter.HighlightTiles(_selectionInfo.rangeV2,
                        TileHighlightColor.InTargetRange);
                }

                public override void Start()
                {
                    _parent._input.EnableMapNaviInput();
                }

                private void OnConfirm(UI.UI_SkillSelectionMenuItem item)
                {
                    if (_parent._battleData.currentUnit == null) return;
                    
                }

                private SelectionInfo GetRangeTilesFrom(int gx, int gy, int totalRange, bool ignoreTerrain, bool includeSelf)
                {
                    Queue<BattleMapTile> queue = new Queue<BattleMapTile>();
                    HashSet<BattleMapTile> explored = new HashSet<BattleMapTile>();
                    Dictionary<BattleMapTile, float> tileCostLeft = new Dictionary<BattleMapTile, float>();
                    Dictionary<BattleMapTile, BattleMapTile> tileParents = new Dictionary<BattleMapTile, BattleMapTile>();
                    List<BattleMapTile> rangeTiles = new List<BattleMapTile>();

                    BattleMap battleMap = _parent._battleData._battleMap;
                    BattleMapTile startTile = battleMap.GetValue(gx, gy);
                    explored.Add(startTile);
                    queue.Enqueue(startTile);
                    tileCostLeft.Add(startTile, totalRange);

                    while (queue.Count != 0)
                    {
                        BattleMapTile currentTile = queue.Dequeue();
                        rangeTiles.Add(currentTile);
                        explored.Add(currentTile);
                        foreach (Vector2 tileCoord in battleMap.GetValue(currentTile.x, currentTile.y).neighbours)
                        {
                            BattleMapTile tile = battleMap.GetValue(tileCoord);
                            float costLeft = tileCostLeft[currentTile] - (ignoreTerrain ? 1 : tile.cost);
                            if (!explored.Contains(tile) && costLeft >= 0f) // TODO: && currentTile.CanGoto(tile))
                            {
                                queue.Enqueue(tile);

                                if (!tileParents.ContainsKey(tile))
                                    tileParents.Add(tile, currentTile);

                                if (!tileCostLeft.ContainsKey(tile))
                                    tileCostLeft.Add(tile, costLeft);

                                tileCostLeft[tile] = costLeft;
                            }
                        }
                    }

                    if (!includeSelf)
                        rangeTiles.Remove(startTile);

                    return SelectionInfo.From(rangeTiles, tileParents);
                }

                private struct SelectionInfo
                {
                    public List<BattleMapTile> rangeTiles { get; private set; }
                    public List<Vector2> rangeV2 { get; private set; }
                    public Dictionary<BattleMapTile, BattleMapTile> tileParents { get; private set; }
                    public static SelectionInfo From(List<BattleMapTile> _rangeTiles, Dictionary<BattleMapTile, BattleMapTile> _tileParents) => new SelectionInfo
                    {
                        rangeTiles = _rangeTiles,
                        rangeV2 = _rangeTiles.Select(tile => new Vector2(tile.x, tile.y)).ToList(),
                        tileParents = _tileParents,
                    };
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