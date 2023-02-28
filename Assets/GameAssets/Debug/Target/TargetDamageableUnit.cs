using Assets.UnityFoundation.DamagePopup.Scripts;
using UnityEngine;
using UnityFoundation.Code.UnityAdapter;
using UnityFoundation.HealthSystem;

namespace GameAssets
{
    public class TargetDamageableUnit : MonoBehaviour, IDamageableUnit
    {
        [SerializeField] private DamageableLayer damageableLayer;

        public IDamageable Damageable => HealthSystem;

        public IHealthSystem HealthSystem { get; private set; }

        public string Name => "Target";

        public ITransform Transform { get; private set; }

        public UnitFactions Faction => UnitFactions.Enemy;

        public ITransform ProjectileHit => Transform;

        public void Awake()
        {
            Transform = transform.Decorate();

            var healthSystem = new HealthSystem();
            healthSystem.Setup(999);
            healthSystem.SetDamageableLayer(damageableLayer);

            healthSystem.OnTakeDamageAmount += (amount) => {
                var popup = DamagePopup.Create(amount.ToString(), Transform.Position);
                popup.SetFontSize(2f);
            };
            healthSystem.OnDied += () => Destroy(gameObject);

            HealthSystem = healthSystem;
        }
    }
}
