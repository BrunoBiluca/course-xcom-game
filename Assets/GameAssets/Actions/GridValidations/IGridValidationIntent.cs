using GameAssets.ActorSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAssets
{
    public interface IGridValidationIntent
    {
        void Validate(ref UnitWorldGridValidator validator);
    }
}
