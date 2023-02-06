using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityFoundation.Code;

namespace GameAssets
{
    public class UnitActionSelector : MonoBehaviour
    {
        private UnitActionsView selector;
        private UnitActionsEnum action;

        public void Setup(UnitActionsView selector, UnitActionsEnum action)
        {
            this.selector = selector;
            this.action = action;

            Create();
        }

        public void Create()
        {
            GetComponent<Button>().onClick.AddListener(Select);

            var text = transform.FindComponent<TextMeshProUGUI>("text");
            text.text = action.ToString();
        }

        public void Select()
        {
            selector.Select(action);
        }

        public void SetColorIfActive(UnitActionsEnum actionType, Color active, Color inactive)
        {
            if(action == actionType)
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
