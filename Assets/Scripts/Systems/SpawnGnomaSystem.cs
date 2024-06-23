using Unity.Burst;
using Unity.Entities;

namespace CPD.Gnoma
{
    [BurstCompile]
    public partial struct SpawnGnomaSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<GnomaSpawnTimer>();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var deltaTime = SystemAPI.Time.DeltaTime;
            var ecbSingleton = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>();

            new SpawnGnomaJob
            {
                DeltaTime = deltaTime,
                ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged)
            }.Run();
        }
    }
    
    [BurstCompile]
    public partial struct SpawnGnomaJob : IJobEntity
    {
        public float DeltaTime;
        public EntityCommandBuffer ECB;
        
        [BurstCompile]
        private void Execute(FieldAspect field)
        {
            field.GnomaSpawnTimer -= DeltaTime;
            if(!field.TimeToSpawnGnoma) return;
            if(!field.GnomaSpawnPointInitialized()) return;
            
            field.GnomaSpawnTimer = field.GnomaSpawnRate;
            var newGnoma = ECB.Instantiate(field.GnomaPrefab);

            var newGnomaTransform = field.GetGnomaSpawnPoint();
            ECB.SetComponent(newGnoma, newGnomaTransform);
            
            var gnomaHeading = MathHelpers.GetHeading(newGnomaTransform.Position, field.Position);
            ECB.SetComponent(newGnoma, new GnomaHeading{Value = gnomaHeading});
        }
    }
}