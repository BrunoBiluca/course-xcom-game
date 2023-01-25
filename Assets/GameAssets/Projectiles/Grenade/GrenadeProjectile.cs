using System;
using UnityEngine;
using UnityFoundation.Code;
using UnityFoundation.Code.UnityAdapter;

namespace GameAssets
{
    public class GrenadeProjectile : IProjectile
    {
        public event Action OnReachTarget;

        private IProjectile.Settings config;
        private ITransform projectile;
        private Vector3 startPos;
        private Vector3 midPoint;
        private Vector3 endPos;
        private float interpolateAmount;

        private bool reachedTarget;

        public void Setup(IProjectile.Settings config)
        {
            this.config = config;
            projectile = config.Transform;
            startPos = projectile.Position;
            midPoint = (config.Transform.Position + config.TargetPos) / 2f;
            midPoint.y = 2f;
            endPos = config.TargetPos;

            reachedTarget = false;
            interpolateAmount = 0f;
        }

        public void Update(float interpolateTime = 1)
        {
            if(reachedTarget) return;

            interpolateAmount += interpolateTime * config.Speed;
            projectile.Position = LinearInterpolation.Quadratic(
                startPos,
                midPoint,
                endPos,
                interpolateAmount
            );

            reachedTarget = projectile.Position == endPos;
            if(reachedTarget)
                OnReachTarget?.Invoke();
        }
    }
}
