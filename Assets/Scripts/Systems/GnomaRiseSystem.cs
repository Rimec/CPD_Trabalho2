using Unity.Burst;
using Unity.Entities;

namespace CPD.Gnoma
{
    [BurstCompile]
    [UpdateAfter(typeof(SpawnGnomaSystem))]
    public partial struct GnomaRiseSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
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
            
            new GnomaRiseJob
            {
                DeltaTime = deltaTime,
                ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter()
            }.ScheduleParallel();
        }
    }

    [BurstCompile]
    public partial struct GnomaRiseJob : IJobEntity
    {
        public float DeltaTime;
        public EntityCommandBuffer.ParallelWriter ECB;
        
        [BurstCompile]
        private void Execute(GnomaRiseAspect gnoma, [ChunkIndexInQuery]int sortKey)
        {
            gnoma.Rise(DeltaTime);
            if(!gnoma.IsAboveGround) return;
            
            gnoma.SetAtGroundLevel();
            ECB.RemoveComponent<GnomaRiseRate>(sortKey, gnoma.Entity);
            ECB.SetComponentEnabled<GnomaWalkProperties>(sortKey, gnoma.Entity, true);
        }
    }

}