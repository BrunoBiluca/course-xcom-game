using System;
using UnityEngine;
using UnityFoundation.Code;
using UnityFoundation.Code.Grid;

namespace GameAssets
{


    [CreateAssetMenu(menuName = "Xcom Game/Level Setup", fileName = "new_level_setup_config")]
    public class LevelSetupConfigSO : ScriptableObject
    {
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
