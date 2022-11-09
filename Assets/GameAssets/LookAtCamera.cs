using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAssets
{
    public class LookAtCamera : MonoBehaviour
    {
        void Update()
        {
            transform.LookAt(Camera.main.transform);
        }
    }
}
