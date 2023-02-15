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
            ICharacterUnit unit, Vector3 target, Action callback
        )
        {
            unit.Transform.LookAt(new Vector3(target.x, 0f, target.z));

            if(unit.RightShoulder != null)
            {
                action.OnFinishAction += HideActionCamera;
                ViewsManager.I.AllViewsHide();
                CameraManager.I.ShowActionCamera(unit.RightShoulder.Position, target);
                AsyncProcessor.I.ProcessAsync(callback, 1f);
                return;
            }
            callback();
        }

        private void HideActionCamera()
        {
            action.OnFinishAction -= HideActionCamera;
            CameraManager.I.HideActionCamera(1f);
            ViewsManager.I.AllViewsShow();
        }
    }
}
