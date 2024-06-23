using Unity.Entities;
using UnityEngine;

namespace CPD.Gnoma
{
    public class GnomaMono : MonoBehaviour
    {
        public float RiseRate;
        public float WalkSpeed;
        public float WalkAmplitude;
        public float WalkFrequency;
        
        public float EatDamage;
        public float EatAmplitude;
        public float EatFrequency;
    }

    public class GnomaBaker : Baker<GnomaMono>
    {
        public override void Bake(GnomaMono authoring)
        {
            var gnomaEntity = GetEntity(TransformUsageFlags.Dynamic);
            
            AddComponent(gnomaEntity, new GnomaRiseRate { Value = authoring.RiseRate });
            AddComponent(gnomaEntity, new GnomaWalkProperties
            {
                WalkSpeed = authoring.WalkSpeed,
                WalkAmplitude = authoring.WalkAmplitude,
                WalkFrequency = authoring.WalkFrequency
            });
            AddComponent(gnomaEntity, new GnomaEatProperties
            {
                EatDamagePerSecond = authoring.EatDamage,
                EatAmplitude = authoring.EatAmplitude,
                EatFrequency = authoring.EatFrequency
            });
            AddComponent<GnomaTimer>(gnomaEntity);
            AddComponent<GnomaHeading>(gnomaEntity);
            AddComponent<NewGnomaTag>(gnomaEntity);
        }
    }
}