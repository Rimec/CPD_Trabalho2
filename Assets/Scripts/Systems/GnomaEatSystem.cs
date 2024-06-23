using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace CPD.Gnoma
{
    [BurstCompile]
    [UpdateAfter(typeof(GnomaWalkSystem))]
    public partial struct GnomaEatSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<TreeTag>();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;
            var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            var treeEntity = SystemAPI.GetSingletonEntity<TreeTag>();
            var treeScale = SystemAPI.GetComponent<LocalTransform>(treeEntity).Scale;
            var treeRadius = treeScale * 5f + 1f;
            
            new GnomaEatJob
            {
                DeltaTime = deltaTime,
                ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
                TreeEntity = treeEntity,
                TreeRadiusSq = treeRadius * treeRadius
            }.ScheduleParallel();
        }
    }

    [BurstCompile]
    public partial struct GnomaEatJob : IJobEntity
    {
        public float DeltaTime;
        public EntityCommandBuffer.ParallelWriter ECB;
        public Entity TreeEntity;
        public float TreeRadiusSq;
        
        [BurstCompile]
        private void Execute(GnomaEatAspect gnoma, [ChunkIndexInQuery]int sortKey)
        {
            if (gnoma.IsInEatingRange(float3.zero, TreeRadiusSq))
            {
                gnoma.Eat(DeltaTime, ECB, sortKey, TreeEntity);
            }
            else
            {
                ECB.SetComponentEnabled<GnomaEatProperties>(sortKey, gnoma.Entity, false);
                ECB.SetComponentEnabled<GnomaWalkProperties>(sortKey, gnoma.Entity, true);
            }
        }
    }

}