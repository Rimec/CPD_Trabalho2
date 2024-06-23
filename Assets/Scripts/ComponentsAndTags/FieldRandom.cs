using Unity.Entities;
using Unity.Mathematics;

namespace CPD.Gnoma
{
    public struct FieldRandom : IComponentData
    {
        public Random Value;
    }
}