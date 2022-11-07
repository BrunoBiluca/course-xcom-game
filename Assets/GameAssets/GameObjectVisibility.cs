using UnityFoundation.Code;

namespace GameAssets
{
    public class GameObjectVisibility : IVisible
    {
        public IGameObject gameObject;

        public bool StartVisible { get; set; }

        public GameObjectVisibility(IGameObject gameObject)
        {
            this.gameObject = gameObject;

            if(StartVisible) Show();
            else Hide();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
