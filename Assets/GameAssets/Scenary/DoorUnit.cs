using UnityEngine;
using UnityFoundation.Code.UnityAdapter;

namespace GameAssets
{
    public class DoorUnit : MonoBehaviour, IInteractableUnit
    {
        public string Name => name;

        public bool IsBlockable { get; private set; }

        public ITransform Transform => transform.Decorate();

        public UnitFactions Faction => UnitFactions.Furniture;

        private Animator animator;

        public void Awake()
        {
            IsBlockable = true;
            animator = GetComponent<Animator>();
            Animate();
        }

        public void Interact()
        {
            IsBlockable = !IsBlockable;
            Animate();
        }

        public void Animate()
        {
            animator.SetBool("isOpen", !IsBlockable);
        }
    }
}
