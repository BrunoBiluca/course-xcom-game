using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameAssets
{
    public class UnitWorldGridActionValidation
    {
        public void SetAction(GridUnitAction unitAction)
        {
            unitAction.ApplyValidation();
        }


    }
}
