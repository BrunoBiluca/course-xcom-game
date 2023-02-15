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
        : BaseView
        , IDependencySetup<IActorSelector<IAPActor>>
    {
        private IActorSelector<IAPActor> actorSelector;
        private TextMeshProUGUI text;

        protected override void OnAwake()
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
            if(actorSelector.CurrentUnit == null) return;

            var actor = actorSelector.CurrentUnit;
            text.text = $"Action Points: {actor.ActionPoints.CurrentAmount}";
        }
    }
}
