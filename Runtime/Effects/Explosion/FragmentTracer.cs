using System.Collections.Generic;
using System.Linq;
using ScriptableResourceLoader;
using UnityEngine;
using WeaponSystem.API;
using Random = UnityEngine.Random;

namespace WeaponSystem.Effects.Explosion
{
    public class FragmentTracer : MonoBehaviour
    {
        [SerializeField] private int _fragmentCount = 40;
        [SerializeField] private float _explosionRadius = 5.0f;
        [SerializeField] private int _fragmentRandom;
        [SerializeField] private float _fragmentDamage = 10.0f;
#if UNITY_EDITOR
        private Vector3[] _fragmentTraces;
        private List<Vector3> tracesHit;
        private readonly int _maxColliders = 50;
        private LayerMask _layerMask;

        private void Awake()
        {
            _layerMask = ResourceLoader.GetResource<WeaponSystemPreferences>().ProjectileLayerMask;

        }

        private void OnDrawGizmos()
        {
            if (_fragmentTraces == null)
            {
                _fragmentTraces = ComputeFragmentTraces();
            }

            Gizmos.color = Color.red;

            if (_fragmentTraces == null) return;
            foreach (var fragmentTrace in _fragmentTraces)
            {
                Gizmos.DrawLine(transform.position, transform.position + fragmentTrace);
                Gizmos.DrawSphere(transform.position + fragmentTrace, 0.1f);
            }

            if (tracesHit != null)
            {
                Gizmos.color = Color.blue;
                foreach (var trace in tracesHit)
                {
                    Gizmos.DrawLine(transform.position, trace);
                }
            }
        }
#endif
        public void Compute()
        {
            var traces = ComputeFragmentTraces();
#if UNITY_EDITOR
            tracesHit = new List<Vector3>();
            _fragmentTraces = traces;
#endif
            var hitColliders = new Collider[_maxColliders];

            // Use Physics.OverlapSphere to get all colliders within the explosion radius
            var size = Physics.OverlapSphereNonAlloc(transform.position, _explosionRadius, hitColliders, _layerMask);

            var hitboxes = hitColliders.Where(h=>h?.gameObject.GetComponents<ImpactConsumer>()!=null).ToArray();
            foreach (var hitCollider in hitboxes)
            {
                var hitPoint = hitCollider.transform.position;
                var direction = (hitPoint - transform.position).normalized;
                var distance = Vector3.Distance(transform.position, hitPoint);
                var ray = new Ray(transform.position, direction);

                // Test for obstacles between this and hitbox
                if (Physics.Raycast(ray, out var hit, distance, _layerMask))
                {
#if UNITY_EDITOR
                    Debug.Log(hit.collider.gameObject);
                    tracesHit.Add(hit.point);
#endif
                    var consumer = hit.collider.gameObject.GetComponent<ImpactConsumer>();
                    if (consumer)
                    {
                        distance = distance > 0 ? distance : 0.1f;
                        consumer.Consume(_fragmentDamage);
                    }
                }
            }

        }


        private Vector3[] ComputeFragmentTraces()
        {
            var fragmentTraces = new List<Vector3>();

            for (var i = 0; i < _fragmentCount + Random.Range(-_fragmentRandom, _fragmentRandom); i++)
            {
                // Calculate a random direction within the explosion radius
                var fragmentDirection = Random.onUnitSphere * _explosionRadius;
                fragmentTraces.Add(fragmentDirection);
            }

            return fragmentTraces.ToArray();
        }
    }
}