using System.Collections;
using UnityEngine;
using WeaponSystem.API;

namespace WeaponSystem.Effects
{
    [RequireComponent(typeof(AudioSource))]
    public class SmokeEffect : ExplosionEffectBase
    {
        [SerializeField, InspectorName("Duration (in seconds)")] private float _duration;
        [SerializeField] private AudioClip _initialSound;
        private AudioSource _audioSource;
        private Transform _vfxTransform;

        private void Awake()
        {
            _audioSource = gameObject.GetComponent<AudioSource>();
            _vfxTransform = VisualEffect.transform;
        }
        private void Update()
        {
            _vfxTransform.rotation = Quaternion.identity;
        }
        public override void Explode()
        {
            StartCoroutine(InitSounds());
            VisualEffect.enabled = true;
            StartCoroutine(WaitUntilDead());
        }

        private IEnumerator InitSounds()
        {
            _audioSource.clip = _initialSound;
            var waitSeconds = _initialSound.length;
            _audioSource.Play();
            yield return new WaitForSeconds(waitSeconds);
            _audioSource.clip = SoundEffect;
            _audioSource.loop = true;
            _audioSource.Play();
        }


        private IEnumerator WaitUntilDead()
        {
            yield return new WaitForSeconds(_duration);
            VisualEffect.enabled = false;
            yield return new WaitForSeconds(60);
            DestroyImmediate(gameObject);
        }
    }
}