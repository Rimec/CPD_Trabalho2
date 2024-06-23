using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace CPD.Gnoma
{
    public readonly partial struct GnomaEatAspect : IAspect
    {
        public readonly Entity Entity;
        
        private readonly RefRW<LocalTransform> _transform;
       
        private readonly RefRW<GnomaTimer> _gnomaTimer;
        private readonly RefRO<GnomaEatProperties> _eatProperties;
        private readonly RefRO<GnomaHeading> _heading;

        private float EatDamagePerSecond => _eatProperties.ValueRO.EatDamagePerSecond;
        private float EatAmplitude => _eatProperties.ValueRO.EatAmplitude;
        private float EatFrequency => _eatProperties.ValueRO.EatFrequency;
        private float Heading => _heading.ValueRO.Value;
        
        private float GnomaTimer
        {
            get => _gnomaTimer.ValueRO.Value;
            set => _gnomaTimer.ValueRW.Value = value;
        }
        
        public void Eat(float deltaTime, EntityCommandBuffer.ParallelWriter ecb, int sortKey, Entity treeEntity)
        {
            GnomaTimer += deltaTime;
            var eatAngle = EatAmplitude * math.sin(EatFrequency * GnomaTimer);
            _transform.ValueRW.Rotation = quaternion.Euler(eatAngle, Heading, 0);

            var eatDamage = EatDamagePerSecond * deltaTime;
            var curTreeDamage = new TreeDamageBufferElement { Value = eatDamage };
            ecb.AppendToBuffer(sortKey, treeEntity, curTreeDamage);
        }
        
        public bool IsInEatingRange(float3 treePosition, float treeRadiusSq)
        {
            return math.distancesq(treePosition, _transform.ValueRO.Position) <= treeRadiusSq - 1;
        }
    }
}