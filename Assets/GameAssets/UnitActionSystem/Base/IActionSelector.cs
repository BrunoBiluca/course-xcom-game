using System;
using UnityFoundation.Code;

namespace GameAssets
{
    public interface IActionSelector<TAction> where TAction : IAPActionIntent
    {
        public event Action<TAction> OnActionSelected;
        public event Action OnActionUnselected;

        public Optional<TAction> CurrentAction { get; }

        public void SetAction(TAction action);
        void UnselectAction();
    }
}