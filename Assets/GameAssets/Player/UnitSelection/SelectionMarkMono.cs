using UnityEngine;
using UnityFoundation.Code.DebugHelper;

namespace GameAssets
{
    public class SelectionMarkMono : MonoBehaviour, IBilucaLoggable
    {
        [SerializeField] private GameObject selectionMark;

        private ISelectable selectable;

        public IBilucaLogger Logger { get; set; }

        public void Start()
        {
            selectable = GetComponent<ISelectable>();

            selectable.OnSelectedStateChange += SelectedStateHandler;
        }

        private void SelectedStateHandler()
        {
            if(selectionMark == null)
                selectionMark = transform.Find("selection_mark").gameObject;

            if(selectionMark != null)
            {
                Logger?.LogWarning($"Selection mark not found on {transform.name}");
                selectionMark.SetActive(selectable.IsSelected);
            }

        }
    }
}
