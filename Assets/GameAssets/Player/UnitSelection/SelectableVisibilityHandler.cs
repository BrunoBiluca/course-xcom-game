using UnityEngine;
using UnityFoundation.Code;
using UnityFoundation.Code.DebugHelper;

namespace GameAssets
{
    public class SelectableVisibilityHandler : IBilucaLoggable
    {
        public IBilucaLogger Logger { get; set; }

        private readonly IGameObject[] gameObjects;

        public SelectableVisibilityHandler(params IGameObject[] gameObjects)
        {
            this.gameObjects = gameObjects;
        }

        public SelectableVisibilityHandler(
            ISelectable selectable,
            params IGameObject[] gameObjects
        ) : this(gameObjects)
        {
            selectable.OnSelected += Show;
            selectable.OnUnselected += Hide;
        }

        public void Show()
        {
            foreach(var gameObject in gameObjects)
            {
                gameObject.SetActive(true);
                Logger?.LogHighlight(gameObject.Name, "was shown");
            }
        }

        public void Hide()
        {
            foreach(var gameObject in gameObjects)
            {
                gameObject.SetActive(false);
                Logger?.LogHighlight(gameObject.Name, "was hidden");
            }
        }
    }
}
