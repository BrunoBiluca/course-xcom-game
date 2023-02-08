using System;
using UnityEngine;
using UnityFoundation.Code;
using UnityFoundation.Code.UnityAdapter;

namespace GameAssets
{
    public class LerpProjectile : IProjectile
    {
        public event Action OnReachTarget;

        private IProjectile.Settings config;

        private ITransform projectile;
        private Vector3 startPos;
        private Vector3 endPos;
        private bool reachedTarget = false;
        private float interpolateAmount;

        public void Setup(IProjectile.Settings config)
        {
            this.config = config;
            projectile = config.Transform;

            startPos = config.Transform.Position;
            endPos = config.TargetPos;

            interpolateAmount = 0f;
        }

        public void Update(float interpolateTime = 1)
        {
            if(reachedTarget) return;

            interpolateAmount += interpolateTime * config.Speed;
            projectile.Position = Vector3.Lerp(startPos, endPos, interpolateAmount);

            reachedTarget = projectile.Position == endPos;
            if(reachedTarget)
                OnReachTarget?.Invoke();
        }
    }
}
