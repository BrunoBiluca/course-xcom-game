using System;
using UnityEngine;
using UnityFoundation.Code;
using UnityFoundation.Code.Grid;

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

    [CreateAssetMenu(menuName = "Xcom Game/Level Setup", fileName = "new_level_setup_config")]
    public class LevelSetupConfig : ScriptableObject
    {
        [field: SerializeField] public GridXZConfig GridConfig { get; private set; }

        [NonReorderable]
        public UnitSetupConfig[] Units;

        [NonReorderable]
        public EnemySetupConfig[] Enemies;

        [SerializeField] public ActionPointsConfig actionPointsConfig;


        [Serializable]
        public class EnemySetupConfig
        {
            public GameObject EnemyPrefab;
            public UnitConfigTemplate UnitTemplate;
            public GridXZConfig.Position Position;
        }

        [Serializable]
        public class UnitSetupConfig
        {
            public GameObject prefab;
            public UnitConfigTemplate UnitTemplate;
            public GridXZConfig.Position Position;
        }
    }
}
