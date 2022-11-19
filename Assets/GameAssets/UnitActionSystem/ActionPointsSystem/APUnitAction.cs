using System;
using System.Collections;
using UnityEngine;

namespace GameAssets
{
    public class APUnitAction : IAPUnitAction
    {
        private readonly IUnitAction action;

        public bool ExecuteImmediatly => action.ExecuteImmediatly;

        public int ActionPointsCost { get; set; }

        public event Action OnCantExecuteAction;
        public event Action OnFinishAction;

        public APUnitAction(IUnitAction action)
        {
            this.action = action;
        }

        public void ApplyValidation()
        {
            action.ApplyValidation();
        }

        public void Execute()
        {
            action.Execute();
        }
    }
}