using Moq;
using UnityEngine;
using UnityFoundation.Code;

namespace GameAssets.Tests
{
    public class ProjectileFactoryMock
    {
        public Mock<IProjectile> Projectile { get; private set; }
        public IProjectileFactory ProjectileFactory { get; private set; }

        public void RaiseProjectileReachedTarget()
        {
            Projectile.Raise(mock => mock.OnReachTarget += null);
        }

        public void Build()
        {
            var projectileFactory = new Mock<IProjectileFactory>();
            Projectile = new Mock<IProjectile>();
            projectileFactory
                .Setup(pf => pf.Create(It.IsAny<Vector3>(), It.IsAny<Vector3>()))
                .Returns(Projectile.Object);

            ProjectileFactory = projectileFactory.Object;
        }
    }
}