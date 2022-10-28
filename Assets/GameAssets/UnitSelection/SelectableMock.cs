using System;
using UnityEngine;

namespace GameAssets
{
    public class SelectableMock : MonoBehaviour, ISelectable
    {
        public bool State { get; private set; }

        public bool IsSelected => State;

        public event Action OnSelectedStateChange;

        public Collider GetCollider()
        {
            return GetComponent<Collider>();
        }

        public void SetSelected(bool state)
        {
            State = state;
        }
    }
}