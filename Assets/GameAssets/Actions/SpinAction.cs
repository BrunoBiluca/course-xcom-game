using System;
using UnityEngine;
using UnityFoundation.Code;
using UnityFoundation.Code.UnityAdapter;

namespace GameAssets
{
    public class SpinUnitAction : IUnitAction, IBilucaLoggable
    {
        public IBilucaLogger Logger { get; set; }

        private readonly IUnitActor actor;
        private readonly ITransform transform;
        private LerpAngle unitRotation;

        public event Action OnFinishAction;

        public SpinUnitAction(
            IUnitActor actor,
            ITransform transform
        )
        {
            this.actor = actor;
            this.transform = transform;
        }

        public bool ExecuteImmediatly => true;

        public void ApplyValidation()
        {
        }

        public void Execute()
        {
            Logger?.LogHighlight(nameof(SpinUnitAction), "execute");

            var initialAngle = transform.Rotation.eulerAngles.y;
            unitRotation = new LerpAngle(initialAngle) {
                CheckMinPath = false
            }
                .SetInterpolationSpeed(300)
                .SetEndValue(initialAngle + 360f);

            actor.SetUpdateCallback(RotateUnit);
        }

        private void RotateUnit(float updateTime)
        {
            var newAngle = unitRotation.EvalAngle(updateTime);

            if(newAngle >= unitRotation.EndValue)
            {
                actor.ResetUpdateCallback();
                OnFinishAction?.Invoke();
                Logger?.LogHighlight(nameof(SpinUnitAction), "finish");
                return;
            }

            transform.Rotation = Quaternion.Euler(0f, newAngle, 0f);
        }
    }
}
