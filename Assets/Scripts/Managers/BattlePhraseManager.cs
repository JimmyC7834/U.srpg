using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Game.Battle.Map;
using Game.Unit;
using Game.Unit.Skill;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.UIElements;

namespace Game.Battle
{
    public enum BattleTeam
    {
        Player,
        CPU,
    }
    
    public class BattlePhraseManager : MonoBehaviour
    {
        public class Phrase
        {
            private BattlePhraseManager _parent;
            private BattleService battleService => _parent._battleService;
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

            public class HandleKokuPhrase : Phrase
            {
                public HandleKokuPhrase(BattlePhraseManager parent) : base(parent) { }

                public override void Start()
                {
                    while (battleService.unitManager.currentKokuUnits.Count == 0)
                    {
                        battleService.battleTurnManager.NextKoku();
                    }

                    List<UnitObject> cpus =
                        battleService.unitManager.currentKokuUnits.Where(unit => unit.cpuUnitController.haveAI).ToList();

                    _parent.Pop();
                    if (cpus.Count != 0)
                    {
                        _parent.Push(new CpuActionPhrase(_parent, cpus));
                    }
                    else
                    {
                        _parent.Push(new UnitSelectionPhrase(_parent));
                    }
                }
            }

            public class CpuActionPhrase : Phrase
            {
                private List<UnitObject> _units;
                private List<CpuActionInfo> _currentActions;
                private SkillCastInfo _skillCastInfo;
                private SkillCaster _skillCaster;

                public CpuActionPhrase(BattlePhraseManager parent, List<UnitObject> units) : base(parent)
                {
                    _units = units;
                    _skillCaster = new SkillCaster(battleService);
                }

                public override void Enter()
                {
                    _input.DisableAllInput();
                }

                public override void Start()
                {
                    GetActionsForNextCpu();
                    ExecuteCurrentActions();
                }

                private void GetActionsForNextCpu()
                {
                    if (_units.Count == 0)
                    {
                        EndPhrase();
                        return;
                    }

                    UnitObject unit = _units[0];
                    CpuUnitController cpu = unit.GetComponent<CpuUnitController>();
                    _currentActions = cpu.GetNextActions();
                }

                private void ExecuteCurrentActions()
                {
                    if (_currentActions == null || _currentActions.Count == 0) return;
                    ExecuteNextAction();
                }

                private void ExecuteNextAction()
                {
                    if (_currentActions.Count == 0) return;
                    UnitObject unit = _units[0];
                    CpuActionInfo action = _currentActions[0];
                    _currentActions.RemoveAt(0);
                    
                    _skillCastInfo = new SkillCastInfo(
                        battleService.battleBoard.GetTile(unit.gridX, unit.gridY), action.skill);
                    _skillCastInfo.SetTargetTile(action.targetTile);
                    _skillCaster.Initialize(_skillCastInfo);
                    _skillCaster.CastSkill(ExecuteNextAction);
                }

                private void NextCpu()
                {
                    _units.RemoveAt(0);
                }

                private void EndPhrase()
                {
                    
                }
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
                    UnitObject unit = battleService.CurrentUnitObject;
                    if (unit == null) return;
                    if (unit._team != BattleTeam.Player) return;
                    if (!battleService.unitManager.currentKokuUnits.Contains(unit)) return;
                    Debug.Log($"Confrimed on unit {battleService.CurrentTile}");
                    _parent.Pop();
                    _parent.Push(new SkillSelectionPhrase(_parent));
                }

                public override string ToString() => "UnitSelectionPhrase";
            }
            
            public class SkillSelectionPhrase : Phrase
            {
                public SkillSelectionPhrase(BattlePhraseManager parent) : base(parent) { }
                
                public override void Enter()
                {
                    _input.DisableAllInput();
                    _input.menuXEvent += ActionEnd;
                }

                public override void Start()
                {
                    battleService.uiManager.OpenSkillSelectionMenu(
                        battleService.CurrentUnitObject,
                        (item) =>
                        {
                            SkillConfirmed(item.skill);
                        });
                    
                    _input.EnableMenuNaviInput();
                }

                private void SkillConfirmed(SkillSO skill)
                {
                    _parent.Pop();
                    _parent.Push(new TargetSelectionPhrase(_parent, skill));
                }

                private void ActionEnd()
                {
                    battleService.unitManager.currentKokuUnits.Remove(battleService.CurrentUnitObject);
                    battleService.CurrentUnitObject.EndAction();
                    battleService.uiManager.CloseSkillSelectionMenu();
                    // battleService.unitManager.UpdateCurrentKokuUnits(battleService.currentKoku);
                    _parent.Pop();
                    if (battleService.unitManager.currentKokuUnits.Count == 0)
                    {
                        _parent.Push(new HandleKokuPhrase(_parent));
                        return;
                    }
                    
                    _parent.Push(new UnitSelectionPhrase(_parent));
                }

                public override void Exit()
                {
                    _input.menuXEvent -= ActionEnd;
                }
            }
            
            public class TargetSelectionPhrase : Phrase
            {
                private SkillCaster _skillCaster;
                private SkillSO _skill;
                private SkillCastInfo _skillCastInfo;

                public TargetSelectionPhrase(BattlePhraseManager parent, SkillSO skill) : base(parent)
                {
                    _skill = skill;
                }

                public override void Enter()
                {
                    _input.DisableAllInput();
                    _skillCaster = new SkillCaster(battleService);
                    _skillCastInfo = new SkillCastInfo(battleService.CurrentTile, _skill);
                    _skillCaster.Initialize(_skillCastInfo);
                    _skillCaster.HighlightRange();
                }

                public override void Start()
                {
                    _input.EnableMapNaviInput();
                    _cursor.OnConfirm += OnConfirm;
                }

                private void OnConfirm(CursorController cursor)
                {
                    if (!_skillCaster.Castable()) return;
                    _cursor.OnConfirm -= OnConfirm;
                    
                    _input.DisableAllInput();
                    _parent.Pop();
                    SkillAnimationPhrase skillAnimationPhrase = new SkillAnimationPhrase(_parent);
                    _parent.Push(skillAnimationPhrase);
                    _skillCastInfo.SetTargetTile(battleService.CurrentTile);
                    _skillCaster.CastSkill(skillAnimationPhrase.EndPhrase);
                }
            }
            
            public class SkillAnimationPhrase : Phrase
            {
                public SkillAnimationPhrase(BattlePhraseManager parent) : base(parent) { }

                public override void Enter()
                {
                    _input.DisableAllInput();
                }

                public void EndPhrase()
                {
                    _parent.Pop();
                    _parent.Push(new UnitSelectionPhrase(_parent));
                }
            }
        }

        [SerializeField] private InputReader _input;
        private BattleService _battleService;
        private Phrase _top => _stack.Peek();
        private CursorController _cursor => _battleService.cursor;
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

        public void Clear() => _stack.Clear();
        public bool IsEmpty() => _stack.Count == 0;

        public void Initialize(BattleService battleService)
        {
            _battleService = battleService;

            _stack = new Stack<Phrase>();
            // empty phrase to skip null check
            _stack.Push(new Phrase(this));
            
            Push(new Phrase.HandleKokuPhrase(this));
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