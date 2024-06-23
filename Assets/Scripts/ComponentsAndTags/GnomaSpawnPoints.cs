using Unity.Entities;
using Unity.Mathematics;

namespace CPD.Gnoma
{
    public struct GnomaSpawnPoints : IComponentData
    {
        public BlobAssetReference<GnomaSpawnPointsBlob> Value;
    }

    public struct GnomaSpawnPointsBlob
    {
        public BlobArray<float3> Value;
    }
}