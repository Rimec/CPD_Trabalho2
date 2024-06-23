using Unity.Entities;

namespace CPD.Gnoma
{
    [InternalBufferCapacity(8)]
    public struct TreeDamageBufferElement : IBufferElementData
    {
        public float Value;
    }
}