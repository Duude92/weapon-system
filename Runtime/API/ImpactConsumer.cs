using UnityEngine;

namespace WeaponSystem.API
{
    public abstract class ImpactConsumer : MonoBehaviour
    {
        public abstract void Consume(float damage);
    }
}