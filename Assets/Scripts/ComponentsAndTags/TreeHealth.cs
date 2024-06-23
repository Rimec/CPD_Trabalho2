using Unity.Entities;

namespace CPD.Gnoma
{
    public struct TreeHealth : IComponentData
    {
        public float Value;
        public float Max;
    }
}