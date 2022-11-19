using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAssets
{
    public class SelectableMono : MonoBehaviour, ISelectable
    {
        public bool IsSelected => throw new NotImplementedException();

        public event Action OnSelected;
        public event Action OnUnselected;
        public event Action OnSelectedStateChange;

        public void SetSelected(bool state)
        {
            throw new NotImplementedException();
        }
    }
}
