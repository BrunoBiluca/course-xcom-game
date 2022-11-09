using UnityEngine;
using UnityFoundation.Code;

namespace GameAssets
{
    public class CameraManager : Singleton<CameraManager>
    {
        [SerializeField] private VisibilityHandlerMono actionCamera;

        public void ShowActionCamera(Vector3 position, Vector3 direction)
        {
            actionCamera.transform.position = position;
            actionCamera.transform.LookAt(direction);
            actionCamera.Show();
        }

        public void HideActionCamera()
        {
            actionCamera.Hide();
        }
    }
}
