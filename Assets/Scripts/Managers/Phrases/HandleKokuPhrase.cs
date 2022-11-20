using System.Collections.Generic;
using System.Linq;
using Game.Unit;

namespace Game.Battle
{
    public class HandleKokuPhrase : Phrase
    {
        public HandleKokuPhrase(BattlePhraseManager parent) : base(parent)
        {
        }

        public override void Start()
        {
            _input.DisableAllInput();

            while (battleService.unitManager.currentKokuUnits.Count == 0)
            {
                battleService.battleTurnManager.NextKoku();
            }

            List<UnitObject> cpus =
                battleService.unitManager.currentKokuUnits.Where(unit => unit.cpuUnitController.haveAI)
                    .ToList();

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