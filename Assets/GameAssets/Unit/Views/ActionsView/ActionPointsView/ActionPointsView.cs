using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityFoundation.CharacterSystem.ActorSystem;
using UnityFoundation.Code;
using UnityFoundation.Code.DebugHelper;

namespace GameAssets
{
    public class ActionPointsView
        : MonoBehaviour
        , IDependencySetup<IActorSelector<IAPActor>>
    {
        private IActorSelector<IAPActor> actorSelector;
        private TextMeshProUGUI text;

        public void Awake()
        {
            text = transform.FindComponent<TextMeshProUGUI>("text");
        }

        public void Setup(IActorSelector<IAPActor> actorSelector)
        {
            this.actorSelector = actorSelector;
        }

        public void Update()
        {
            if(actorSelector == null) return;
            if(actorSelector.CurrentUnitActor == null) return;

            var actor = actorSelector.CurrentUnitActor;
            text.text = $"Action Points: {actor.ActionPoints.CurrentAmount}";
        }
    }
}
