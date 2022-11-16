using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityFoundation.Code.DebugHelper;

namespace GameAssets
{
    public class ResetUnitsPositions : MonoBehaviour, IDebuggerAction
    {
        public string Name => "Reset units";

        public void Execute()
        {
            var unitsManager = FindObjectOfType<UnitsManager>();

            unitsManager.ResetUnits();
        }
    }
}
