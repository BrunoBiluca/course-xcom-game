using System;
using UnityEngine;
using UnityFoundation.Code;
using UnityFoundation.Code.UnityAdapter;

namespace GameAssets
{
    public class TransformNavegationAgent : INavegationAgent
    {
        private readonly ITransform transform;
        private Optional<Vector3> target;
        private float rotateSpeed;

        public event Action OnReachDestination;

        public TransformNavegationAgent(ITransform transform)
        {
            this.transform = transform;

            target = Optional<Vector3>.None();
            Speed = 1f;
            PositionInterpolation = 1f;
            rotateSpeed = 10f;
        }

        public Vector3 CurrentPosition => transform.Position;

        public float PositionInterpolation { get; private set; }
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
            transform.Position += Speed * PositionInterpolation * moveDirection.normalized;

            transform.Foward = Vector3.Lerp(
                transform.Foward, moveDirection, Time.deltaTime * rotateSpeed
            );

            if(DistanceMagnitude() <= StoppingDistance)
            {
                OnReachDestination?.Invoke();
                ResetPath();
                return;
            }
        }

        /// <summary>
        /// Update the transform navegation using a custom updateTime. 
        /// Can be used with Time.deltaTime to smothly update every frame.
        /// </summary>
        /// <param name="updateTime">1f means full speed every update</param>
        public void UpdateWithTime(float updateTime = 1f)
        {
            PositionInterpolation = updateTime;
            Update();
        }

        private float DistanceMagnitude()
        {
            var pos = target.OrElse(CurrentPosition);
            return (pos - CurrentPosition).magnitude;
        }
    }
}