using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAssets
{
    public class TurnSystem : ITurnSystem
    {
        public int CurrentTurn { get; private set; }

        public event Action OnTurnEnded;

        public TurnSystem()
        {
            CurrentTurn = 1;
        }

        public void EndTurn()
        {
            CurrentTurn++;
            OnTurnEnded?.Invoke();
        }
    }
}
