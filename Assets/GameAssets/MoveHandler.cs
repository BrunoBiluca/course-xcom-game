using System;
using UnityEngine;
using UnityFoundation.Code;
using UnityFoundation.Code.UnityAdapter;

namespace GameAssets
{
    public class MoveHandler : INavegationAgent
    {
        private readonly ITransform transform;
        private Optional<Vector3> target;
        private float updateTime;

        public MoveHandler(ITransform transform)
        {
            this.transform = transform;

            target = Optional<Vector3>.None();
            Speed = 1f;
            updateTime = 1f;
        }

        public Vector3 CurrentPosition => transform.Position;

        public float Speed { get; set; }

        public float StoppingDistance { get; set; }

        public float RemainingDistance => DistanceMagnitude();

        public void Disabled()
        {
            throw new System.NotImplementedException();
        }

        public void ResetPath()
        {
            target = Optional<Vector3>.None();
        }

        public bool SetDestination(Vector3 targetPosition)
        {
            target = Optional<Vector3>.Some(targetPosition);
            return true;
        }

        public void Update()
        {
            if(!target.IsPresentAndGet(out Vector3 destination)) return;

            var moveDirection = (destination - CurrentPosition);
            transform.Position += Speed * updateTime * moveDirection.normalized;

            if(DistanceMagnitude() < StoppingDistance)
            {
                ResetPath();
                return;
            }
        }

        public void UpdateWithTime(float updateTime = 1f)
        {
            this.updateTime = updateTime;
            Update();
        }

        private float DistanceMagnitude()
        {
            var pos = target.OrElse(CurrentPosition);
            return (pos - CurrentPosition).magnitude;
        }
    }
}