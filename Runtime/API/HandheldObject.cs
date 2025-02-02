using UnityEngine;

namespace ClientAPI
{
    [RequireComponent(typeof(Collider)), RequireComponent(typeof(Rigidbody)),RequireComponent(typeof(HandheldComponent))]
    public class HandheldObject : MonoBehaviour, IThrowable
    {
        protected Rigidbody RigidBody { get; private set; }
        protected bool FirstShotTriggered;

        public virtual void Use(bool isInitiated, Vector3 forward)
        {
            if (!isInitiated && FirstShotTriggered)
            {
                FirstShotTriggered = false;
                //trigger throwing animation
                Throw(forward, 0);
            }

            if (!FirstShotTriggered && isInitiated)
            {
                //trigger swing animation
            }

            if (!FirstShotTriggered) FirstShotTriggered = isInitiated;
        }

        public virtual void SecondaryUse(bool isInitiated, Vector3 forward) => Use(isInitiated, forward);

        protected virtual void Awake()
        {
            RigidBody = GetComponent<Rigidbody>();
            RigidBody.isKinematic = true;
        }

        public virtual void Throw(Vector3 forward, float force)
        {
            IThrowable.RaiseThrownEvent(this);

            transform.parent = null;
            RigidBody.isKinematic = false;
            RigidBody.AddForce(forward * force);
        }
    }
}