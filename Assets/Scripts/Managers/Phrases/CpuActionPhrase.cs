using System.Collections.Generic;
using Game.Unit;

namespace Game.Battle
{
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
                    if (_currentActions == null || _currentActions.Count == 0)
                    {
                        NextCpu();
                        return;
                    }
                    
                    ExecuteNextAction();
                }

                private void ExecuteNextAction()
                {
                    UnitObject unit = _units[0];
                    if (_currentActions.Count == 0)
                    {
                        battleService.unitManager.currentKokuUnits.Remove(unit);
                        unit.EndAction();
                        NextCpu();
                        return;
                    }
                    
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
                    if (_units.Count == 0)
                    {
                        EndPhrase();
                        return;
                    }
                    
                    GetActionsForNextCpu();
                    ExecuteCurrentActions();
                }

                private void EndPhrase()
                {
                    _parent.Pop();

                    
                    _parent.Push(new UnitSelectionPhrase(_parent));
                }
            }
}