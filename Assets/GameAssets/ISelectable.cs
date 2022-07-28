using UnityEngine;

namespace GameAssets
{
    public interface ISelectable
    {
        Collider GetCollider();
        void SetSelected(bool state);
    }
}