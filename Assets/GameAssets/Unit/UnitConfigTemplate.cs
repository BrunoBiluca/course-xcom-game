using UnityEngine;

namespace GameAssets
{
    [CreateAssetMenu(menuName = "Xcom Game/Unit/Config", fileName = "new_unit_config_template")]
    public class UnitConfigTemplate : ScriptableObject
    {
        [field: SerializeField] public int MovementRange { get; private set; }
        [field: SerializeField] public uint MaxActionPoints { get; private set; }
    }
}
