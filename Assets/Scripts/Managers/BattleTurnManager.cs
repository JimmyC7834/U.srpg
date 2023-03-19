using System;
using UnityEngine;

namespace Game.Battle {
    /**
     * Manage progress of kokus and turns of the battle
     */
    public class BattleTurnManager : MonoBehaviour
    {
        [SerializeField] private int _kokuPerTurn;
        public int koku { get; private set; }
        public int turn { get; private set; }

        public event Action<int> OnTurnChanged = delegate {  };
        public event Action<int> OnKokuChanged = delegate {  };

        public void Initialize()
        {
            koku = _kokuPerTurn;
            OnKokuChanged.Invoke(koku);
            OnTurnChanged.Invoke(turn);
        }

        public void NextKoku()
        {
            koku--;
            if (koku <= 0)
                NextTurn();
            OnKokuChanged.Invoke(koku);
        }
    
        public void NextTurn()
        {
            turn++;
            koku = _kokuPerTurn;
            OnTurnChanged.Invoke(turn);
        }
    }
}