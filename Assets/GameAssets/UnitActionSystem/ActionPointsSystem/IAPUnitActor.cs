using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityFoundation.ResourceManagement;

namespace GameAssets
{
    public interface IAPUnitActor : IUnitActor<IAPUnitAction>
    {
        IResourceManager ActionPoints { get; }
    }
}
