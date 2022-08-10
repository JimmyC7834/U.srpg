using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTurnManager : MonoBehaviour
{
    [SerializeField] private int _kokuPerTurn;
    public int koku { get; private set; }
    public int turn { get; private set; }

    public event Action<int> OnTurnChanged;
    public event Action<int> OnKokuChanged;

    public void Awake()
    {
        koku = _kokuPerTurn;
        OnKokuChanged?.Invoke(koku);
        OnTurnChanged?.Invoke(turn);
    }

    public void NextKoku()
    {
        koku--;
        if (koku <= 0)
        {
            NextTurn();
        }
        OnKokuChanged?.Invoke(koku);
        Debug.Log($"koku changed to: {koku}");
    }
    
    public void NextTurn()
    {
        turn++;
        koku = _kokuPerTurn;
        OnTurnChanged?.Invoke(turn);
    }
}
