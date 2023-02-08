using UnityFoundation.HealthSystem;

namespace GameAssets
{
    public interface IDamageableUnit : IUnit
    {
        IDamageable Damageable { get; }
        IHealthSystem HealthSystem { get; }
    }
}
