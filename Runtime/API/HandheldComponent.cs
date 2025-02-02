
using System;
using UnityEngine;

namespace ClientAPI
{
    public class HandheldComponent : MonoBehaviour
    {
        [field: SerializeField] public Transform LeftHandTransform { get; private set; }

        [field: SerializeField] public Transform RightHandTransform { get; private set; }

        public void SecondaryUse(bool isInitiated, Vector3 forward) => AdditionalComponent?.SecondaryUse(isInitiated, forward);

        public void Use(bool isInitiated, Vector3 forward) => AdditionalComponent?.Use(isInitiated, forward);
        [field: SerializeField] public HandheldObject AdditionalComponent { get; private set; }
        [field: SerializeField] public AnimatorOverrideController AnimatorOverrideController { get; private set; }

        private void Awake()
        {
            if (!AdditionalComponent)
                AdditionalComponent = gameObject.AddComponent<HandheldObject>();
        }
    }
}