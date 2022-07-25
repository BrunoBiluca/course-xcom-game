using UnityEngine;
using UnityFoundation.Code;

namespace GameAssets
{
    public interface IWorldCursor
    {
        Optional<Vector3> WorldPosition { get; }

        void Update();
    }
}