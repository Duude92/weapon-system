using UnityEngine;
using WeaponSystem.API;

namespace WeaponSystem
{
    public class ProjectileShootProcessor :WeaponShootProcessor
    {
        [SerializeField] private Projectile _projectilePrefab;
        [SerializeField] private float _bulletMass = 1f;
        [SerializeField] private float _bulletForce = 1f;

 

        public override void Shoot()
        {
            if (_projectilePrefab != null)
            {
                var projectile = Projectile.Create(FirePlace, _bulletMass, _bulletForce, _projectilePrefab);
                projectile.PlaySound();
            }
        }
    }
}