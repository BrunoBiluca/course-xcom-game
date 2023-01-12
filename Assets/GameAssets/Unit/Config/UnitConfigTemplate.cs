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
            int initialHealth
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
    }

    public class UnitConfigBuilder
    {
        public Sprite Portrait { get; set; }
        public string Name { get; set; }
        public int InitialHealth { get; private set; }
        public int MovementRange { get; set; }
        public uint MaxActionPoints { get; set; }
        public int ShootDamage { get; private set; }
        public int ShootRange { get; set; }
        public int GrenadeRange { get; set; }
        public int MeleeRange { get; set; }

        public UnitConfig Build()
        {
            return new UnitConfig(null, null, MovementRange, MaxActionPoints, ShootDamage, ShootRange, 0, 0, InitialHealth);
        }
    }
}
