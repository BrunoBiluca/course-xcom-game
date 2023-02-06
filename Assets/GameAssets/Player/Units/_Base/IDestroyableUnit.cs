using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAssets
{
    public interface IDestroyableUnit : IUnit
    {
        void Destroy();
    }
}
