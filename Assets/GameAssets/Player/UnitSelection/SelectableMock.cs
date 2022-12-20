using System;
using UnityEngine;

namespace GameAssets
{
    public class SelectableMock : MonoBehaviour, ISelectable
    {
        public bool State { get; private set; }

        public bool IsSelected => State;

        public event Action OnSelectedStateChange;
        public event Action OnSelected;
        public event Action OnUnselected;

        public void SetSelected(bool state)
        {
            State = state;
            OnSelectedStateChange?.Invoke();
        }
    }
}