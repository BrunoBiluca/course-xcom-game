using System;
using UnityEngine;
using UnityFoundation.Code;

namespace GameAssets
{
    [Serializable]
    public class ActionsDictionary : SerializableDictionary<string, int> { }

    [Serializable]
    public class ActionsConfig
    {
        [SerializeField] private ActionsDictionary Costs;
        public ActionsConfig()
        {
            Costs = new ActionsDictionary();

            foreach(var action in Enum.GetValues(typeof(UnitActionsEnum)))
            {
                Costs.Add(action.ToString(), 0);
            }
        }

        public void AddCost(UnitActionsEnum actionEnum, int amount)
        {
            Costs[actionEnum.ToString()] = amount;
        }

        public int GetCost(UnitActionsEnum actionEnum)
        {
            return Costs[actionEnum.ToString()];
        }
    }
}
