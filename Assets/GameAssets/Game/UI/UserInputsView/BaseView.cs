using UnityEngine;

namespace GameAssets
{
    public abstract class BaseView : MonoBehaviour, IView
    {
        public bool IsVisible => gameObject.activeInHierarchy;

        [field: SerializeField] public bool StartVisible { get; set; } = true;

        private bool wasFirstShown = false;

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            OnShow();

            if(!wasFirstShown)
            {
                wasFirstShown = true;
                OnFirstShow();
            }
        }

        public void Awake()
        {
            OnAwake();

            if(StartVisible) Show();
            else Hide();
        }

        protected virtual void OnAwake() { }
        protected virtual void OnShow() { }
        protected virtual void OnFirstShow() { }
    }
}
