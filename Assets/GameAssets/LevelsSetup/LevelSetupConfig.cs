using System;
using UnityEngine;
using UnityFoundation.Code.Grid;

namespace GameAssets
{
    [CreateAssetMenu(menuName = "Xcom Game/Level Setup", fileName = "new_level_setup_config")]
    public class LevelSetupConfig : ScriptableObject
    {
        [field: SerializeField] public GridXZConfig GridConfig { get; private set; }

        [NonReorderable]
        public UnitSetupConfig[] Units;

        [NonReorderable]
        public EnemySetupConfig[] Enemies;


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
