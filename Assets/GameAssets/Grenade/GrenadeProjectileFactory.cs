using UnityFoundation.Code;

namespace GameAssets
{

    public class GrenadeProjectileFactory : BaseProjectileFactory<GrenadeProjectile>
    {
        protected override float ProjectileSpeed() => 5f;
    }
}
