using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace CPD.Gnoma
{
    [BurstCompile]
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial struct InitializeGnomaSystem : ISystem
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
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            foreach (var gnoma in SystemAPI.Query<GnomaWalkAspect>().WithAll<NewGnomaTag>())
            {
                ecb.RemoveComponent<NewGnomaTag>(gnoma.Entity);
                ecb.SetComponentEnabled<GnomaWalkProperties>(gnoma.Entity, false);
                ecb.SetComponentEnabled<GnomaEatProperties>(gnoma.Entity, false);
            }

            ecb.Playback(state.EntityManager);
        }
    }
}