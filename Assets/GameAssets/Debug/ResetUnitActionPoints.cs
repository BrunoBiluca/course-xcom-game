using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityFoundation.Code.DebugHelper;

namespace GameAssets
{
    public class ResetUnitActionPoints : MonoBehaviour, IDebuggerAction
    {
        public string Name => "Reset units action points";

        public void Execute()
        {
            var units = FindObjectOfType<UnitsManager>().GetAllUnits();

            foreach(var u in units)
            {
                u.Actor.ActionPoints.FullReffil();
            }
        }
    }
}
