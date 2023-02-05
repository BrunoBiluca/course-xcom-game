using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityFoundation.Code.Grid;

namespace GameAssets
{
    public class UnitWorldGridXZ : WorldGridXZMono<UnitValue>
    {
        protected override UnitValue InstantiateValue()
        {
            return new UnitValue();
        }
    }
}
