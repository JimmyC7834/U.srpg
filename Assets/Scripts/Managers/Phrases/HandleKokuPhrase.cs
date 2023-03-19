using System.Collections.Generic;
using Game.Unit;

namespace Game.Battle
{
    public class HandleKokuPhrase : Phrase
    {
        private List<UnitObject> _currentUnits;
        
        public HandleKokuPhrase(BattlePhraseManager parent) : base(parent) { }

        public override void Start()
        {
            _input.DisableAllInput();

            while (battleService.unitManager.NoCurrentUnits())
            {
                battleService.battleTurnManager.NextKoku();
            }
        
            _currentUnits = battleService.unitManager.GetCurrentUnits();
            List<UnitObject> cpus = _currentUnits.FindAll(unit => unit.cpuUnitController.haveAI);
            
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
}