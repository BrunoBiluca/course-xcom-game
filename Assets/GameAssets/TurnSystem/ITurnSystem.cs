using System;

namespace GameAssets
{
    public interface ITurnSystem
    {
        int CurrentTurn { get; }

        event Action OnTurnEnded;

        void EndTurn();
    }
}
