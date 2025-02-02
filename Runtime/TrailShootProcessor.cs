using System;
using ScriptableResourceLoader;
using UnityEngine;
using WeaponSystem.API;

namespace WeaponSystem
{
    public class TrailShootProcessor : WeaponShootProcessor
    {
        [SerializeField] private float _rayDistance = 1000;
        [SerializeField] private ParticleSystem _trailParticle;
        [SerializeField] private float _trailSpeed = 10f;
        private LayerMask _layerMask;

        private void Awake()
        {
            _layerMask = ResourceLoader.GetResource<WeaponSystemPreferences>().ProjectileLayerMask;
        }

        public override void Shoot()
        {
            var trail = GameObject.Instantiate(_trailParticle, FirePlace.position, FirePlace.rotation);
            var mover = trail.gameObject.AddComponent<TrailShootParticleMover>();
            mover.Speed = _trailSpeed;
            if (!Physics.Raycast(FirePlace.position, FirePlace.forward, out RaycastHit hit,
                    _rayDistance,
                    _layerMask))
            {
                mover.End = FirePlace.position + FirePlace.forward * _rayDistance;
                mover.StartMove();
                return;
            }

            mover.End = hit.point;
            mover.StartMove();
            if (!hit.collider.TryGetComponent<HitboxProcessor>(out var processor)) return;
            if (processor)
            {
                processor.Consume(100);
            }
        }
    }
}