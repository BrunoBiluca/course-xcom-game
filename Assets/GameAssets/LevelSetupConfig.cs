using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityFoundation.Code;

namespace GameAssets
{
    [CreateAssetMenu(menuName = "Xcom Game/Level Setup", fileName = "new_level_setup_config")]
    public class LevelSetupConfig : ScriptableObject
    {
        [field: SerializeField] public GridXZConfig GridConfig { get; private set; }

        [field: SerializeField] public UnitSetupConfig[] Units { get; private set; }

        [Serializable]
        public class UnitSetupConfig
        {
            [field: SerializeField] public UnitConfigTemplate UnitTemplate { get; private set; }
            [field: SerializeField] public int PositionX { get; private set; }
            [field: SerializeField] public int PositionZ { get; private set; }
        }
    }
}
