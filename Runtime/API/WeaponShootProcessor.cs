using UnityEngine;

namespace WeaponSystem.API
{
    public abstract class WeaponShootProcessor : MonoBehaviour
    {
        private Transform _firePlace;

        
        public Transform FirePlace
        {
            get => _firePlace;
            set => _firePlace = value;
        }
        public abstract void Shoot();
    }
}