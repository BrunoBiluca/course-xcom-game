using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityFoundation.Code;

namespace GameAssets
{
    public class VisibilityHandlerSingleton 
        : Singleton<VisibilityHandlerSingleton>, IVisibilityHandler
    {
        private readonly VisibilityHandler visibilityHandler = new();

        public void Add(IGameObject gameObject)
        {
            visibilityHandler.Add(gameObject);
        }

        public void Hide()
        {
            visibilityHandler.Hide();
        }

        public void Show()
        {
            visibilityHandler.Show();
        }
    }
}
