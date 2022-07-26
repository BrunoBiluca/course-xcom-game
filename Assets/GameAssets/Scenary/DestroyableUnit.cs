﻿using UnityEngine;
using UnityFoundation.Code;
using UnityFoundation.Code.UnityAdapter;

namespace GameAssets
{
    public class DestroyableUnit : BilucaMono, IDestroyableUnit
    {
        [SerializeField] private GameObject destroyedObjPrefab;

        public string Name => Transform.Name;

        protected override void OnAwake()
        {
            OnObjectDestroyed += OnDestroyed;
        }

        private void OnDestroyed()
        {
            var go = Instantiate(destroyedObjPrefab, transform.position, Quaternion.identity);
            ApplyForce(go.transform, transform.position, 300, 100);
        }

        private void ApplyForce(Transform target, Vector3 position, float force, float radius)
        {
            foreach(Transform child in target.GetChildren())
            {
                if(child.TryGetComponent(out Rigidbody rb))
                    rb.AddExplosionForce(force, position, radius);

                ApplyForce(child, position, force, radius);
            }
        }
    }
}
