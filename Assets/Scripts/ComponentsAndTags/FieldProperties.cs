using Unity.Entities;
using Unity.Mathematics;

namespace CPD.Gnoma
{
    public struct FieldProperties : IComponentData
    {
        public float2 FieldDimensions;
        public int NumberMushroomsToSpawn;
        public Entity MushroomPrefab;
        public Entity GnomaPrefab;
        public float GnomaSpawnRate;
    }
    public struct GnomaSpawnTimer : IComponentData
    {
        public float Value;
    }
}