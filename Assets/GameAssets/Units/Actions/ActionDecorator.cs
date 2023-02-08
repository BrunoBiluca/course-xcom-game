using System;
using UnityEngine;
using UnityFoundation.CharacterSystem.ActorSystem;
using UnityFoundation.Code;
using UnityFoundation.Code.UnityAdapter;

namespace GameAssets
{
    public class ActionDecorator
    {
        private readonly IAction action;

        public ActionDecorator(IAction action)
        {
            this.action = action;
        }

        public void LookAtTarget(
            ICharacterUnit unit, ITransform target, Action callback
        )
        {
            action.OnFinishAction += HideActionCamera;

            var targetPos = target.Position;
            unit.Transform.LookAt(new Vector3(targetPos.x, 0f, targetPos.z));

            if(unit.RightShoulder != null)
            {
                VisibilityHandlerSingleton.I.Hide();
                CameraManager.I.ShowActionCamera(unit.RightShoulder.Position, targetPos);
                AsyncProcessor.I.ProcessAsync(callback, 1f);
                return;
            }
            callback();
        }

        private void HideActionCamera()
        {
            action.OnFinishAction -= HideActionCamera;
            CameraManager.I.HideActionCamera(1f);
        }
    }
}
