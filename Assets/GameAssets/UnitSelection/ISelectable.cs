using System;
using UnityEngine;

namespace GameAssets
{
    public interface ISelectable
    {
        event Action OnSelectedStateChange;

        bool IsSelected { get; }

        Collider GetCollider();
        void SetSelected(bool state);
    }
}