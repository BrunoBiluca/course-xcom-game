using System;
using UnityEngine;

namespace GameAssets
{
    public interface ISelectable
    {
        event Action OnSelected;
        event Action OnUnselected;
        event Action OnSelectedStateChange;

        bool IsSelected { get; }
        void SetSelected(bool state);
    }
}