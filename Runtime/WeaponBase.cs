using ClientAPI;
using UnityEngine;

namespace WeaponSystem
{
    public abstract class WeaponBase : HandheldObject
    {
        // public virtual void SecondaryUse() { }

        // public abstract void Use(bool isInitiated);
        [field: SerializeField] public WeaponType WeaponType { get; private set; }
        [SerializeField] public AnimPoses _animPoses;
        public abstract void SetAim(bool isAim);

    }
    [System.Serializable]
    public struct AnimPoses
    {
        [Header("Idle")]
        public Vector3 IdlePosition;
        public Vector3 IdleRotation;
        [Header("Aim")]
        public Vector3 AimPosition;
        public Vector3 AimRotation;
    }
    public enum WeaponType
    {
        None,
        Rifle,
        RocketLauncher,
        Grenade
    }
}