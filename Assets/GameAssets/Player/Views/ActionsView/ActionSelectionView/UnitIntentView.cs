using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityFoundation.Code;

namespace GameAssets
{
    public class UnitIntentView : MonoBehaviour
    {
        private UnitIntentsView selector;
        public UnitActionsEnum Action { get; private set; }

        public void Setup(UnitIntentsView selector, UnitActionsEnum action)
        {
            this.selector = selector;
            Action = action;

            Create();
        }

        public void Create()
        {
            GetComponent<Button>().onClick.AddListener(Select);

            var text = transform.FindComponent<TextMeshProUGUI>("text");
            text.text = Action.ToString();
        }

        public void Select()
        {
            selector.Select(Action);
        }

        public void SetColorIfActive(UnitActionsEnum actionType, Color active, Color inactive)
        {
            if(Action == actionType)
                SetColor(active);
            else
                SetColor(inactive);
        }

        public void SetColor(Color color)
        {
            GetComponent<Image>().color = color;
        }
    }
}
