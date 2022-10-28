using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                u.ActionPoints.FullReffil();
            }
        }
    }
}
