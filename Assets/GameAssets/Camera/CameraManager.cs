using Cinemachine;
using UnityEngine;
using UnityFoundation.Code;
using UnityFoundation.Code.Timer;
using UnityFoundation.Code.UnityAdapter;

namespace GameAssets
{
    public class CameraManager : Singleton<CameraManager>
    {
        [SerializeField] private VisibilityHandlerMono actionCamera;

        public void ShakeCamera()
        {
            GetComponent<CinemachineImpulseSource>().GenerateImpulse();
        }

        public void ShowActionCamera(Vector3 position, Vector3 direction)
        {
            actionCamera.transform.position = position;
            actionCamera.transform.LookAt(direction);
            actionCamera.Show();
        }

        public void HideActionCamera(float delay)
        {
            var timer = new Timer(delay, () => actionCamera.Hide()).RunOnce();
            timer.Start();
        }
    }
}
