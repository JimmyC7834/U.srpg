using System.Collections.Generic;
using UnityEngine.Assertions;
using Game.Unit;
using Game.Unit.Skill;

namespace Game.Battle
{
    public class CpuActionPhrase : Phrase
    {
        private Queue<CpuUnitController> _cpus;
        private Queue<CpuActionInfo> _actions;

        public CpuActionPhrase(BattlePhraseManager parent, List<UnitObject> units) : base(parent)
        {
            _cpus = new Queue<CpuUnitController>();
            _actions = new Queue<CpuActionInfo>();
            foreach (UnitObject unit in units)
            {
                // if (unit.cpuUnitController.haveAI)
                _cpus.Enqueue(unit.GetComponent<CpuUnitController>());
            }
        }

        public override void Enter()
        {
            _input.DisableAllInput();
        }

        public override void Start()
        {
            RunCpuActions(_cpus.Peek());
        }

        private void NextCpu()
        {
            Assert.IsFalse(_cpus.Count == 0);
            // End Current Cpu Action
            CpuUnitController cpu = _cpus.Dequeue();
            battleService.unitManager.ReturnToHeap(cpu.unit);
            cpu.unit.Data.EndAction();
            
            // End Phrase if not Cpus left
            if (_cpus.Count == 0)
            {
                EndPhrase();
                return;
            }
            
            // Continue to run next cpu action
            RunCpuActions(_cpus.Peek());
        }

        private void RunCpuActions(CpuUnitController cpu)
        {
            _actions = cpu.GetNextActions();
            ExecuteNextAction();
        }

        private void ExecuteNextAction()
        {
            if (_actions.Count == 0)
            {
                NextCpu();
                return;
            }
            
            CpuActionInfo action = _actions.Dequeue();
            CpuUnitController cpu = _cpus.Peek();
            
            SkillCast skillCast = new SkillCast(
                battleService.battleBoard, 
                battleService.battleBoard.GetTile(cpu.unit.GridX, cpu.unit.GridY),
                action.skill);
            skillCast.Cast(battleService, action.targetTile, ExecuteNextAction);
        }

        private void EndPhrase()
        {
            _parent.Pop();
            _parent.Push(new UnitSelectionPhrase(_parent));
        }
    }
}