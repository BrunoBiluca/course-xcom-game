using System;
using UnityEngine;
using UnityFoundation.Code;
using UnityFoundation.Code.DebugHelper;
using UnityFoundation.Code.UnityAdapter;

namespace GameAssets
{
    public class SpinUnitAction : IAction, IBilucaLoggable
    {
        public IBilucaLogger Logger { get; set; }

        private readonly IAsyncProcessor asyncProcessor;
        private readonly ITransform transform;
        private LerpAngle unitRotation;

        public event Action OnFinishAction;
        public event Action OnCantExecuteAction;

        public SpinUnitAction(
            IAsyncProcessor asyncProcessor,
            ITransform transform
        )
        {
            this.asyncProcessor = asyncProcessor;
            this.transform = transform;
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

            asyncProcessor.ExecuteEveryFrame(RotateUnit);
        }

        private void RotateUnit(float updateTime)
        {
            var newAngle = unitRotation.EvalAngle(updateTime);

            if(newAngle >= unitRotation.EndValue)
            {
                asyncProcessor.ResetCallbackEveryFrame();
                OnFinishAction?.Invoke();
                Logger?.LogHighlight(nameof(SpinUnitAction), "finish");
                return;
            }

            transform.Rotation = Quaternion.Euler(0f, newAngle, 0f);
        }
    }
}
