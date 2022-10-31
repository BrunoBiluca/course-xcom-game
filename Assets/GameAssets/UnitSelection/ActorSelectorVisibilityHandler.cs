using UnityEngine;
using UnityFoundation.Code;
using UnityFoundation.Code.DebugHelper;

namespace GameAssets
{
    public class ActorSelectorVisibilityHandler : IBilucaLoggable
    {
        public IBilucaLogger Logger { get; set; }

        private readonly IUnitActorSelector actorSelector;
        private readonly IGameObject[] gameObjects;


        public ActorSelectorVisibilityHandler(
            IUnitActorSelector actorSelector,
            params IGameObject[] gameObjects
        )
        {
            this.actorSelector = actorSelector;
            this.gameObjects = gameObjects;

            actorSelector.OnUnitSelected += Show;
            actorSelector.OnUnitDeselected += Hide;
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
