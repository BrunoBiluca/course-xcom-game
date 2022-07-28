using UnityEngine;

namespace GameAssets
{
    public class SelectableMock : MonoBehaviour, ISelectable
    {
        public bool State { get; private set; }

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