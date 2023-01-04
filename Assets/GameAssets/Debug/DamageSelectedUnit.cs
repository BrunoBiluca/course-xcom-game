using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityFoundation.Code.DebugHelper;

namespace GameAssets
{
    public class DamageSelectedUnit : MonoBehaviour, IDebuggerAction
    {
        public string Name => $"Damage selected unit ({DamageAmount})";

        [SerializeField] private UnitSelectionMono unitSelection;
        [SerializeField] private int DamageAmount;

        public void Execute()
        {
            unitSelection.CurrentUnit.Damageable.Damage(DamageAmount);
        }
    }
}
