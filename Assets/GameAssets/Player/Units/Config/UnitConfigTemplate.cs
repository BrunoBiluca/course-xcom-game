using System;
using UnityEngine;

namespace GameAssets
{
    [CreateAssetMenu(menuName = "Xcom Game/Unit/Config", fileName = "new_unit_config_template")]
    public class UnitConfigTemplate : ScriptableObject
    {
        [field: SerializeField] public UnitConfig UnitConfig { get; private set; }

        [field: SerializeField] public SoundEffects SoundEffects { get; private set; }
    }

    [Serializable]
    public class SoundEffects
    {
        [field: SerializeField] public AudioClip Melee { get; private set; }
        [field: SerializeField] public AudioClip Died { get; private set; }
    }

    [Serializable]
    public class UnitConfig
    {
        public UnitConfig(
            Sprite portrait = null,
            string name = "",
            int movementRange = 0,
            uint maxActionPoints = 0,
            int shootDamage = 0,
            int shootRange = 0,
            int grenadeRange = 0,
            int meleeRange = 0,
            int initialHealth = 0,
            int interactRange = 0,
            int meleeDamage = 0,
            int movementSpeed = 0
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
            MovementSpeed = movementSpeed;
        }

        [field: SerializeField] public Sprite Portrait { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public int InitialHealth { get; private set; }
        [field: SerializeField] public int MovementRange { get; private set; }
        [field: SerializeField] public int MovementSpeed { get; private set; }
        [field: SerializeField] public uint MaxActionPoints { get; private set; }
        [field: SerializeField] public int ShootDamage { get; private set; }
        [field: SerializeField] public int ShootRange { get; private set; }
        [field: SerializeField] public int GrenadeRange { get; private set; }
        [field: SerializeField] public int MeleeRange { get; private set; }
        [field: SerializeField] public int MeleeDamage { get; private set; }
        [field: SerializeField] public int InteractRange { get; private set; }
    }
}
