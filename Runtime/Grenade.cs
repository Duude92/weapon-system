using System.Collections;
using ClientAPI;
using UnityEngine;
using UnityEngine.Serialization;
using WeaponSystem.API;

namespace WeaponSystem
{
    [RequireComponent(typeof(Collider)), RequireComponent(typeof(Rigidbody))]
    public class Grenade : HandheldObject
    {
        [FormerlySerializedAs("_explosionEffect")] [SerializeField]
        private ExplosionEffectBase explosionEffectBase;

        [SerializeField, InspectorName("Time before explosion (in seconds)")]
        private float _timeToExplode;

        protected override void Awake()
        {
            base.Awake();
            explosionEffectBase = GetComponent<ExplosionEffectBase>();
        }

        public override void Use(bool isInitiated, Vector3 forward)
        {
            if (!isInitiated && FirstShotTriggered)
            {
                FirstShotTriggered = false;
                //trigger throwing animation
                Throw(forward, 1000);
            }

            if (!FirstShotTriggered && isInitiated)
            {
                //trigger swing animation
            }

            if (!FirstShotTriggered) FirstShotTriggered = isInitiated;
        }

        public override void Throw(Vector3 forward, float force)
        {
            IThrowable.RaiseThrownEvent(this);

            transform.parent = null;
            RigidBody.isKinematic = false;
            RigidBody.AddForce(forward * force);
            StartCoroutine(TriggerExplosion());
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator TriggerExplosion()
        {
            yield return new WaitForSeconds(_timeToExplode);
            explosionEffectBase.Explode();
        }

        public override void SecondaryUse(bool isInitiated, Vector3 forward)
        {
            Use(isInitiated, forward);
        }
    }
}