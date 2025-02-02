using System;
using System.Collections;
using ScriptableResourceLoader;
using UnityEngine;
using UnityEngine.VFX;
using WeaponSystem;

public class Projectile : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody = null;
    [SerializeField] private CapsuleCollider _collider = null;
    [SerializeField] private float _mass = 1f;
    [SerializeField] private float speed = 1f;
    [SerializeField] private AudioSource _audioSource = null;
    [SerializeField] private AudioSource _explosionSound = null;
    [SerializeField] private VisualEffect _explosionEffect = null;
    [SerializeField] private int _destroyAfterSec = 0;
    private LayerMask _layerMask;

    public static Projectile Create(Transform projectileTransform, float mass, float speed,
        Projectile projectilePrefab = null)
    {
        Projectile thisProjectile;
        if (projectilePrefab)
        {
            thisProjectile = Instantiate(projectilePrefab, projectileTransform.position, projectileTransform.rotation);
            thisProjectile._rigidbody.mass = mass;
            thisProjectile._rigidbody.AddForce(projectileTransform.forward, ForceMode.Impulse);
        }
        else
        {
            GameObject thisObject = new GameObject();
            thisObject.transform.position = projectileTransform.position;
            thisObject.transform.rotation = projectileTransform.rotation;

            thisProjectile = thisObject.AddComponent<Projectile>();

            thisProjectile._collider = thisObject.AddComponent<CapsuleCollider>();

            thisProjectile._rigidbody = thisObject.AddComponent<Rigidbody>();
            thisProjectile._rigidbody.mass = mass;
            thisProjectile._rigidbody.AddForce(projectileTransform.forward, ForceMode.Impulse);
        }

        // Debug.Break();
        thisProjectile.CheckColliderInFront(5);
        return thisProjectile;
    }

    private void Awake()
    {
        if (_destroyAfterSec > 0)
            StartCoroutine(DestroyAfterSeconds(_destroyAfterSec));
        _layerMask = ResourceLoader.GetResource<WeaponSystemPreferences>().ProjectileLayerMask;

    }

    private IEnumerator DestroyAfterSeconds(int destroyAfterSec)
    {
        yield return new WaitForSeconds(destroyAfterSec);
        DestroyImmediate(gameObject);
    }

    public void PlaySound()
    {
        if (_audioSource)
        {
            _audioSource.Play();
        }
    }

    private void FixedUpdate()
    {
        CheckColliderInFront(null);
    }

    private void CheckColliderInFront(float? magnitude)
    {
        if (_rigidbody && Physics.Raycast(transform.position, transform.forward, out RaycastHit hit,
                magnitude ?? _rigidbody.linearVelocity.magnitude * Time.fixedDeltaTime,
                _layerMask))
        {
            OnTriggerEnter(hit.collider);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (_rigidbody)
        {
            Gizmos.DrawRay(transform.position,
                _rigidbody.linearVelocity.magnitude > 0 ? _rigidbody.linearVelocity : transform.forward * 5);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_explosionSound)
            _explosionSound.Play();
        if (_explosionEffect)
        {
            _explosionEffect.enabled = true;
            _explosionEffect?.Play();
        }

        if (_audioSource)
        {
            Destroy(_audioSource);
        }

        var processor = other.GetComponent<HitboxProcessor>();
        if (processor)
        {
            processor.Consume(100);
        }

        Destroy(this._collider);
        Destroy(this._rigidbody);
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        OnTriggerEnter(collision.collider);
    }
}