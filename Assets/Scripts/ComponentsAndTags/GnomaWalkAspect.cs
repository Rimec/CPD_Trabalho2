
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace CPD.Gnoma
{
    public readonly partial struct GnomaWalkAspect : IAspect
    {
        public readonly Entity Entity;
        
        private readonly RefRW<LocalTransform> _transform;
        private readonly RefRW<GnomaTimer> _walkTimer;
        private readonly RefRO<GnomaWalkProperties> _walkProperties;
        private readonly RefRO<GnomaHeading> _heading;

        private float WalkSpeed => _walkProperties.ValueRO.WalkSpeed;
        private float WalkAmplitude => _walkProperties.ValueRO.WalkAmplitude;
        private float WalkFrequency => _walkProperties.ValueRO.WalkFrequency;
        private float Heading => _heading.ValueRO.Value;

        private float WalkTimer
        {
            get => _walkTimer.ValueRO.Value;
            set => _walkTimer.ValueRW.Value = value;
        }

        public void Walk(float deltaTime)
        {
            WalkTimer += deltaTime;
            _transform.ValueRW.Position += _transform.ValueRO.Forward() * WalkSpeed * deltaTime;
            
            var swayAngle = WalkAmplitude * math.sin(WalkFrequency * WalkTimer);
            _transform.ValueRW.Rotation = quaternion.Euler(0, Heading, swayAngle);
        }
        
        public bool IsInStoppingRange(float3 treePosition, float treeRadiusSq)
        {
            return math.distancesq(treePosition, _transform.ValueRO.Position) <= treeRadiusSq;
        }
    }
}