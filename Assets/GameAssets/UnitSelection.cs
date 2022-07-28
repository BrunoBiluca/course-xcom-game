using UnityEngine;
using UnityFoundation.Code;
using UnityFoundation.Code.UnityAdapter;

namespace GameAssets
{
    public class UnitSelection
    {
        private readonly IRaycastHandler raycast;
        private LayerMask layerMask;

        public UnitSelection(IRaycastHandler raycast)
        {
            this.raycast = raycast;
            layerMask = 0;
        }

        public UnitSelection SetLayers(LayerMask layerMask)
        {
            this.layerMask = layerMask;
            return this;
        }

        public Optional<ISelectable> Select(Vector3 screenPosition)
        {
            var target = raycast.GetObjectOf<ISelectable>(screenPosition, layerMask);

            return Optional<ISelectable>.Some(target);
        }
    }
}