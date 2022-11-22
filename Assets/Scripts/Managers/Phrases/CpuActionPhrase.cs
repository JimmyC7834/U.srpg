using System.Collections;
using System.Collections.Generic;
using Game.Unit;
using UnityEngine;
using UnityEngine.Assertions;

namespace Game.Battle
{
    public class CpuActionPhrase : Phrase
    {
        private Queue<CpuUnitController> _cpus;
        private Queue<CpuActionInfo> _actions;
        private SkillCastInfo _skillCastInfo;
        private SkillCaster _skillCaster;

        public CpuActionPhrase(BattlePhraseManager parent, List<UnitObject> units) : base(parent)
        {
            _skillCaster = new SkillCaster(battleService);
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
            cpu.unit.EndAction();
            
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
            
            _skillCastInfo = new SkillCastInfo(
                battleService.battleBoard.GetTile(cpu.unit.gridX, cpu.unit.gridY), action.skill);
            _skillCastInfo.SetTargetTile(action.targetTile);
            _skillCaster.Initialize(_skillCastInfo);
            _skillCaster.CastSkill(ExecuteNextAction);
        }

        private void EndPhrase()
        {
            _parent.Pop();
            _parent.Push(new UnitSelectionPhrase(_parent));
        }
    }
}