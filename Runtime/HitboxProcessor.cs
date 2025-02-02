using System;
using UnityEngine;
using WeaponSystem.API;

namespace WeaponSystem
{
    public class HitboxProcessor : ImpactConsumer
    {
        private Collider _collider;
        [SerializeField] private BodyPart _bodyPart;
        [SerializeField] private BodySide _bodySide;
        public Action<HitboxProcessor> ImpactConsume;
        public BodyPart BodyPart => _bodyPart;
        public BodySide BodySide => _bodySide;
        private void Awake()
        {
            _collider = GetComponent<Collider>();
        }





        public override void Consume(float damage)
        {
            ImpactConsume(this);
        }
    }

    public enum BodyPart
    {
        None,
        Head,
        Neck,
        Breast,
        Pelvis,
        UpperArm,
        ForeArm,
        Hand,
        UpperLeg,
        LowerLeg
    }

    public enum BodySide
    {
        Center,
        Right,
        Left
    }
}