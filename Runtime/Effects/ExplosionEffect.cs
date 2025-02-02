using UnityEngine;
using WeaponSystem.API;
using WeaponSystem.Effects.Explosion;

namespace WeaponSystem.Effects
{
    public class ExplosionEffect : ExplosionEffectBase
    {
        private AudioSource _audioSource;
        [SerializeField] private FragmentTracer _fragmentTracer;

        private void Awake()
        {
            _fragmentTracer ??= gameObject.GetComponent<FragmentTracer>();
            _audioSource = gameObject.GetComponent<AudioSource>();
            _audioSource.playOnAwake = false;
            _audioSource.clip = SoundEffect;
        }

        public override void Explode()
        {
            _audioSource.Play();
            VisualEffect.enabled = true;
            _fragmentTracer?.Compute();
        }
    }
}