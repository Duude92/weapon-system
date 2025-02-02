using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

namespace WeaponSystem
{
    public class TrailShootParticleMover : MonoBehaviour
    {
        public Vector3 End { get; set; }
        public float Speed { get; set; }
        private bool _isActive = false;
        private ParticleSystem _particleSystem;
        private ParticleSystemRenderer _particleSystemRenderer;

        public void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
            _particleSystemRenderer = _particleSystem.GetComponent<ParticleSystemRenderer>();
            _particleSystemRenderer.enabled = false;
        }

        public void StartMove()
        {
            _isActive = true;
            _particleSystemRenderer.enabled = true;
        }

        public void FixedUpdate()
        {
            if(!_isActive) return;
            transform.position = Vector3.MoveTowards(transform.position, End, Speed * Time.deltaTime);
            if (transform.position == End) StopMove();
        }

        private void StopMove()
        {
            _isActive = false;
            StartCoroutine(Deactivate());
        }
        //TODO: Use with TrailPool
        private IEnumerator Deactivate()
        {
            yield return new WaitForSeconds(5);
            _particleSystem.Pause();
            _particleSystemRenderer.enabled = false;
        }
    }
}