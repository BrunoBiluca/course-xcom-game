using System;
using UnityEngine;
using UnityFoundation.Code.UnityAdapter;

namespace GameAssets
{
    public interface ISelectable
    {
        Collider GetCollider();
        void SetSelected(bool state);
    }
}