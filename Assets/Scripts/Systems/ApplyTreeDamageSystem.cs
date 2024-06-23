
using Unity.Burst;
using Unity.Entities;

namespace CPD.Gnoma
{
    [BurstCompile]
    [UpdateInGroup(typeof(SimulationSystemGroup), OrderLast = true)]
    [UpdateAfter(typeof(EndSimulationEntityCommandBufferSystem))]
    public partial struct ApplyTreeDamageSystem : ISystem
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
            state.Dependency.Complete();   
            foreach (var tree in SystemAPI.Query<TreeAspect>())
            {
                tree.DamageTree();
            }
        }
    }
}