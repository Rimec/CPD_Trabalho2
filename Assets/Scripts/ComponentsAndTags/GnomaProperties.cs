using Unity.Entities;

namespace CPD.Gnoma
{
    public struct GnomaWalkProperties : IComponentData, IEnableableComponent
    {
        public float WalkSpeed;
        public float WalkAmplitude;
        public float WalkFrequency;
    }
    
    public struct GnomaEatProperties : IComponentData, IEnableableComponent
    {
        public float EatDamagePerSecond;
        public float EatAmplitude;
        public float EatFrequency;
    }
    
    public struct GnomaTimer : IComponentData
    {
        public float Value;
    }

    public struct GnomaHeading : IComponentData
    {
        public float Value;
    }
    
    public struct NewGnomaTag : IComponentData {}
}