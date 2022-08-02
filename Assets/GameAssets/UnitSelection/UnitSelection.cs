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
        public Optional<ISelectable> CurrentUnit {
            get {
                if(
                    !currentUnit.IsPresentAndGet(out ISelectable value)
                    || IsNullOrDestroyed(value)
                )
                {
                    currentUnit = Optional<ISelectable>.None();
                }

                return currentUnit;
            }
            private set { currentUnit = value; }
        }

        public UnitSelection(IRaycastHandler raycast)
        {
            this.raycast = raycast;
            layerMask = 0;
            CurrentUnit = Optional<ISelectable>.None();
        }

        public UnitSelection SetLayers(LayerMask layerMask)
        {
            this.layerMask = layerMask;
            return this;
        }

        public Optional<ISelectable> Select(Vector3 screenPosition)
        {
            var target = raycast.GetObjectOf<ISelectable>(screenPosition, layerMask);

            CurrentUnit.Some(u => u.SetSelected(false));

            var result = Optional<ISelectable>.Some(target);
            if(result.IsPresent)
            {
                result.Get().SetSelected(true);
            }

            CurrentUnit = result;
            return result;
        }

        public static bool IsNullOrDestroyed(object obj)
        {
            if(obj is null) return true;
            return obj is Object && (obj as Object) == null;
        }
    }
}