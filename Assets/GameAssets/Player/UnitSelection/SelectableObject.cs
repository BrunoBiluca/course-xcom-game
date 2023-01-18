using System;
using UnityFoundation.Code;

namespace GameAssets
{
    public class SelectableObject : ISelectable
    {
        public bool IsSelected { get; private set; }
        public object SelectedReference { get; }

        public event Action OnSelected;
        public event Action OnUnselected;
        public event Action OnSelectedStateChange;

        public SelectableObject()
        {
        }

        public SelectableObject(object reference)
        {
            SelectedReference = reference;
        }

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
