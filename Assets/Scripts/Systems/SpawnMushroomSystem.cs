using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace CPD.Gnoma
{
    [BurstCompile]
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct SpawnMushroomSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state){
            state.RequireForUpdate<FieldProperties>();
        }
        [BurstCompile]
        public void OnDestroy(ref SystemState state){}
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            state.Enabled = false;
            var fieldEntity = SystemAPI.GetSingletonEntity<FieldProperties>();
            var field = SystemAPI.GetAspect<FieldAspect>(fieldEntity);
            
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            var mushroomOffset = new float3(0f, -2f, 1f);
            
            var builder = new BlobBuilder(Allocator.Temp);
            ref var spawnPoints = ref builder.ConstructRoot<GnomaSpawnPointsBlob>();
            var arrayBuilder = builder.Allocate(ref spawnPoints.Value, field.NumberMushroomsToSpawn);
            
            for (var i = 0; i < field.NumberMushroomsToSpawn; i++)
            {
                var newMushroom = ecb.Instantiate(field.MushroomPrefab);
                var newMushroomTransform = field.GetRandomMushroomTransform();
                ecb.SetComponent(newMushroom, newMushroomTransform);
                
                var newGnomaSpawnPoint = newMushroomTransform.Position + mushroomOffset;
                arrayBuilder[i] = newGnomaSpawnPoint;
            }

            var blobAsset = builder.CreateBlobAssetReference<GnomaSpawnPointsBlob>(Allocator.Persistent);
            ecb.SetComponent(fieldEntity, new GnomaSpawnPoints{Value = blobAsset});
            builder.Dispose();

            ecb.Playback(state.EntityManager);
        }
    }
}