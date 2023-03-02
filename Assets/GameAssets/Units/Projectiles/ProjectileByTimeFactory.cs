using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityFoundation.Code;
using UnityFoundation.Code.Math;

namespace GameAssets
{
    public class ProjectileByTime : IProjectile
    {
        public event Action OnReachTarget;

        private readonly float time;
        private float currTime;

        public ProjectileByTime()
        {
        }

        public ProjectileByTime(float time)
        {
            this.time = time;
        }

        public void Setup(IProjectile.Settings config)
        {
            config.Transform.LookAt(config.TargetPos.WithY(config.Transform.Position.y));
        }

        public void Update(float interpolateTime = 1)
        {
            if(currTime >= time)
                return;

            currTime += interpolateTime;

            if(currTime >= time)
                OnReachTarget?.Invoke();
        }
    }

    public class ProjectileByTimeFactory : ProjectileFactory<ProjectileByTime>
    {
        [SerializeField] private float timeToReachTarget;

        protected override ProjectileByTime InstantiateProjectile()
        {
            return new ProjectileByTime(timeToReachTarget);
        }
    }
}
