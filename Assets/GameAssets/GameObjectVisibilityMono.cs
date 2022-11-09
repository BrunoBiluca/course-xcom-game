using UnityEngine;
using UnityFoundation.Code;

namespace GameAssets
{
    public class GameObjectVisibilityMono : MonoBehaviour, IVisible
    {
        private GameObjectVisibility visibility;

        [field: SerializeField] public bool StartVisible { get; set; }

        public void Awake()
        {
            visibility = new GameObjectVisibility(
                new GameObjectDecorator(gameObject)
            ) {
                StartVisible = StartVisible
            };
        }

        public void Hide()
        {
            visibility.Hide();
        }

        public void Show()
        {
            visibility.Show();
        }
    }
}
