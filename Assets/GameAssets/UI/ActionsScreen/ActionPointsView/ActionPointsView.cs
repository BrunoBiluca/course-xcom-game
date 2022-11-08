using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityFoundation.Code;
using UnityFoundation.Code.DebugHelper;

namespace GameAssets
{
    public class ActionPointsView : MonoBehaviour
    {
        private IUnitActorSelector actorSelector;
        private TextMeshProUGUI text;

        public void Awake()
        {
            text = transform.FindComponent<TextMeshProUGUI>("text");
        }

        public void Setup(IUnitActorSelector actorSelector)
        {
            this.actorSelector = actorSelector;
        }

        public void Update()
        {
            if(actorSelector.CurrentUnitActor == null) return;

            var actor = (TrooperUnit)actorSelector.CurrentUnitActor;
            text.text = $"Action Points: {actor.ActionPoints.CurrentAmount}";
        }
    }
}
