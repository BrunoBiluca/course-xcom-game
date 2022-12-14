using UnityEngine;

namespace GameAssets
{
    public class SelectionMarkMono : MonoBehaviour
    {
        [SerializeField] private GameObject selectionMark;

        private ISelectable selectable;

        public void Start()
        {
            selectable = GetComponent<ISelectable>();

            selectable.OnSelectedStateChange += SelectedStateHandler;
        }

        private void SelectedStateHandler()
        {
            if(selectionMark == null)
                selectionMark = transform.Find("selection_mark").gameObject;

            selectionMark.SetActive(selectable.IsSelected);
        }
    }
}
