using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.VFX;
using WeaponSystem.API;

namespace WeaponSystem
{
    [RequireComponent(typeof(Collider))]
    public class Weapon : WeaponBase
    {
        [Serializable]
        struct IdleOffset
        {
            public Vector3 positionOffset;
            public Vector3 rotationOffset;
        }

        [Serializable]
        public struct WeaponAudio
        {
            [FormerlySerializedAs("ShotClip")] public AudioClip shotClip;
        }

        private Collider _collider;
        private bool _firing;
        private float _timer;
        [SerializeField] private FireMode _fireMode = FireMode.Single;
        [SerializeField] private Sight _currentSight;
        [SerializeField] private List<Sight> _sights = new List<Sight>();
        [SerializeField] private Transform _firePlace;
        [SerializeField] private float _fireSpeed = 1;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private WeaponAudio _audio;
        private bool _isAiming;
        private WeaponShootProcessor _weaponShootProcessor;

        protected override void Awake()
        {
            _collider = GetComponent<Collider>();
            _weaponShootProcessor = GetComponent<WeaponShootProcessor>();
            _weaponShootProcessor.FirePlace = _firePlace;
            base.Awake();
        }

        public void Picked()
        {
            // _collider.enabled = false;
            // Destroy(_rigidbody);
            if (RigidBody)
                RigidBody.isKinematic = true;
        }

        public void UnPicked()
        {
            _collider.enabled = true;
            gameObject.AddComponent<Rigidbody>();
        }

        public override void SecondaryUse(bool isInitiated, Vector3 forward) => ChangeAim();

        private void ChangeAim()
        {
            _currentSight.Camera.enabled = !_isAiming;
            _isAiming = !_isAiming;
        }

        public override void SetAim(bool isAim)
        {
            _currentSight.Camera.enabled = isAim;
            _isAiming = isAim;
        }

        public override void Use(bool isInitiated, Vector3 forward)
        {
            if (!isInitiated) FirstShotTriggered = isInitiated;
            if (!FirstShotTriggered && isInitiated)
                Fire();
            if (!FirstShotTriggered) FirstShotTriggered = isInitiated;
        }

        private void Fire()
        {
            _weaponShootProcessor.Shoot();

            _audioSource.clip = _audio.shotClip;
            _audioSource?.Play();
        }

        private void Fire(bool startFire)
        {
            _firing = startFire;
        }

        private void LateUpdate()
        {
            if (_firing)
            {
                _timer += Time.deltaTime;
                if (_timer > _fireSpeed)
                {
                    _timer = 0;
                    Fire();
                    if (_fireMode == FireMode.Single)
                    {
                        Fire(false);
                    }
                }
            }
        }

        enum FireMode
        {
            Single,
            Triple,
            Automatic
        }
    }
}