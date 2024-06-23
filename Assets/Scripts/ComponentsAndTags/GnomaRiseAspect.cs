using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace CPD.Gnoma
{
    public readonly partial struct GnomaRiseAspect : IAspect
    {
        public readonly Entity Entity;

        private readonly RefRW<LocalTransform> _transform;
        private readonly RefRO<GnomaRiseRate> _gnomaRiseRate;

        public void Rise(float deltaTime)
        {
            _transform.ValueRW.Position += math.up() * _gnomaRiseRate.ValueRO.Value * deltaTime;
        }
        
        public bool IsAboveGround => _transform.ValueRO.Position.y >= 0f;

        public void SetAtGroundLevel()
        {
            var position = _transform.ValueRO.Position;
            position.y = 0f;
            _transform.ValueRW.Position = position;
        }
    }
}