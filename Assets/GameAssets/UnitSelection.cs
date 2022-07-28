using UnityEngine;
using UnityFoundation.Code;
using UnityFoundation.Code.UnityAdapter;

namespace GameAssets
{
    public class UnitSelection
    {
        private readonly IRaycastHandler raycast;
        private LayerMask layerMask;
        private Optional<ISelectable> currentUnit;

        public UnitSelection(IRaycastHandler raycast)
        {
            this.raycast = raycast;
            layerMask = 0;
            currentUnit = Optional<ISelectable>.None();
        }

        public UnitSelection SetLayers(LayerMask layerMask)
        {
            this.layerMask = layerMask;
            return this;
        }

        public Optional<ISelectable> Select(Vector3 screenPosition)
        {
            var target = raycast.GetObjectOf<ISelectable>(screenPosition, layerMask);

            currentUnit.Some(u => u.SetSelected(false));

            var result = Optional<ISelectable>.Some(target);
            if(result.IsPresent)
            {
                result.Get().SetSelected(true);
            }

            currentUnit = result;
            return result;
        }
    }
}