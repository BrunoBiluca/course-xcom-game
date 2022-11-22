using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityFoundation.ResourceManagement;

namespace GameAssets
{
    public interface IAPActor : IActor<IAPActionIntent>
    {
        IResourceManager ActionPoints { get; }
    }
}
