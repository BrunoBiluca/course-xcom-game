using System;
using UnityEngine;
using UnityFoundation.Code;

namespace GameAssets
{
    [Serializable]
    public class ActionPointsDictionary : SerializableDictionary<string, int> { }

    [Serializable]
    public class ActionPointsConfig
    {
        [SerializeField] private ActionPointsDictionary Costs;
        public ActionPointsConfig()
        {
            Costs = new ActionPointsDictionary();

            foreach(var action in Enum.GetValues(typeof(UnitActionsEnum)))
            {
                Costs.Add(action.ToString(), 0);
            }
        }

        public int GetCost(UnitActionsEnum actionEnum)
        {
            return Costs[actionEnum.ToString()];
        }
    }
}
