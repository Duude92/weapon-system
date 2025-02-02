using UnityEngine;
using UnityEngine.VFX;

namespace WeaponSystem.API
{
    public abstract class ExplosionEffectBase : MonoBehaviour
    {
        [field: SerializeField] public AudioClip SoundEffect { get; private set; }
        [field: SerializeField] public VisualEffect VisualEffect { get; private set; }
        public abstract void Explode();
    }
}