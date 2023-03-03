using UnityEngine;
using UnityFoundation.Code.Extensions;
using UnityFoundation.Code.UnityAdapter;
using UnityFoundation.SettingsSystem;

namespace GameAssets
{
    public enum DoorAnimationEvents
    {
        Open,
        Close
    }

    public class DoorUnit :
        MonoBehaviour,
        IInteractableUnit,
        IAnimationEventHandler<DoorAnimationEvents>
    {
        [SerializeField] private AudioClip doorOpenSound;
        [SerializeField] private AudioClip doorCloseSound;

        public string Name => name;

        public bool IsBlockable { get; private set; }

        public bool IsOpen => !IsBlockable;
        public bool IsClose => IsBlockable;

        public ITransform Transform => transform.Decorate();

        public UnitFactions Faction => UnitFactions.Furniture;

        private Animator animator;
        private SoundEffectsControllerMono soundController;

        public void Awake()
        {
            IsBlockable = true;
            animator = GetComponent<Animator>();
            soundController = GetComponent<SoundEffectsControllerMono>();
        }

        public void Interact()
        {
            IsBlockable = !IsBlockable;
            Animate();
        }

        public void Animate()
        {
            soundController.Play(doorOpenSound);
            animator.SetBool("isOpen", !IsBlockable);
        }

        public void AnimationEventHandler(DoorAnimationEvents value)
        {
            switch(value)
            {
                case DoorAnimationEvents.Open:
                    soundController.Stop();
                    break;
                case DoorAnimationEvents.Close:
                    soundController.Play(doorCloseSound);
                    break;
                default:
                    break;
            }
        }
    }
}
