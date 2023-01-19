using System;
using UnityEngine;

namespace GameAssets
{
    [CreateAssetMenu(menuName = "Xcom Game/Unit/Config", fileName = "new_unit_config_template")]
    public class UnitConfigTemplate : ScriptableObject
    {
        [field: SerializeField] public UnitConfig UnitConfig { get; private set; }
    }

    [Serializable]
    public class UnitConfig
    {
        public UnitConfig(
            Sprite portrait,
            string name,
            int movementRange,
            uint maxActionPoints,
            int shootDamage,
            int shootRange,
            int grenadeRange,
            int meleeRange,
            int initialHealth,
            int interactRange,
            int meleeDamage
        )
        {
            Portrait = portrait;
            Name = name;
            MovementRange = movementRange;
            MaxActionPoints = maxActionPoints;
            ShootDamage = shootDamage;
            ShootRange = shootRange;
            GrenadeRange = grenadeRange;
            MeleeRange = meleeRange;
            InitialHealth = initialHealth;
            InteractRange = interactRange;
            MeleeDamage = meleeDamage;
        }

        [field: SerializeField] public Sprite Portrait { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public int InitialHealth { get; private set; }
        [field: SerializeField] public int MovementRange { get; private set; }
        [field: SerializeField] public uint MaxActionPoints { get; private set; }
        [field: SerializeField] public int ShootDamage { get; private set; }
        [field: SerializeField] public int ShootRange { get; private set; }
        [field: SerializeField] public int GrenadeRange { get; private set; }
        [field: SerializeField] public int MeleeRange { get; private set; }
        [field: SerializeField] public int MeleeDamage { get; private set; }
        [field: SerializeField] public int InteractRange { get; private set; }
    }
}
