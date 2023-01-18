using System;
using UnityEngine;
using UnityFoundation.Code;

namespace GameAssets
{
    public class SelectableMono : MonoBehaviour, ISelectable
    {
        public bool IsSelected { get; private set; }

        public object SelectedReference => this;

        public event Action OnSelected;
        public event Action OnUnselected;
        public event Action OnSelectedStateChange;

        public void SetSelected(bool state)
        {
            IsSelected = state;
            if(IsSelected)
                OnSelected?.Invoke();
            else
                OnUnselected?.Invoke();
            OnSelectedStateChange?.Invoke();
        }
    }
}
