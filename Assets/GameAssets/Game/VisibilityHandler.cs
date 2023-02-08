using System.Collections.Generic;
using System.Linq;
using UnityFoundation.Code;
using UnityFoundation.Code.DebugHelper;

namespace GameAssets
{
    public class VisibilityHandler : IBilucaLoggable, IVisibilityHandler
    {
        public IBilucaLogger Logger { get; set; }

        private readonly List<IGameObject> gameObjects = new();

        public VisibilityHandler()
        {
        }

        public VisibilityHandler(params IGameObject[] gameObjects)
        {
            this.gameObjects = gameObjects.ToList();
        }

        public void Add(IGameObject gameObject)
        {
            gameObjects.Add(gameObject);
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
