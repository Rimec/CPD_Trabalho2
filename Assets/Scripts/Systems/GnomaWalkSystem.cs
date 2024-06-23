using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace CPD.Gnoma
{
    [BurstCompile]
    [UpdateAfter(typeof(GnomaRiseSystem))]
    public partial struct GnomaWalkSystem : ISystem
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
            var treeRadius = treeScale * 5f + 0.5f;
            
            new GnomaWalkJob
            {
                DeltaTime = deltaTime,
                TreeRadiusSq = treeRadius * treeRadius,
                ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter()
            }.ScheduleParallel();
        }
    }

    [BurstCompile]
    public partial struct GnomaWalkJob : IJobEntity
    {
        public float DeltaTime;
        public float TreeRadiusSq;
        public EntityCommandBuffer.ParallelWriter ECB;
        
        
        [BurstCompile]
        private void Execute(GnomaWalkAspect gnoma, [ChunkIndexInQuery] int sortKey)
        {
            gnoma.Walk(DeltaTime);
            if (gnoma.IsInStoppingRange(float3.zero, TreeRadiusSq))
            {
                ECB.SetComponentEnabled<GnomaWalkProperties>(sortKey, gnoma.Entity, false);
                ECB.SetComponentEnabled<GnomaEatProperties>(sortKey, gnoma.Entity, true);
            }
            
        }
    }

}