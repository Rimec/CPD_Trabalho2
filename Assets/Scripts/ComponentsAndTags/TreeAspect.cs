using Unity.Entities;
using Unity.Transforms;

namespace CPD.Gnoma
{
    public readonly partial struct TreeAspect : IAspect
    {
        public readonly Entity Entity;

        private readonly RefRW<LocalTransform> _transform;
        private readonly RefRW<TreeHealth> _treeHealth;
        private readonly DynamicBuffer<TreeDamageBufferElement> _treeDamageBuffer;

        public void DamageTree()
        {
            foreach (var treeDamageBufferElement in _treeDamageBuffer)
            {
                _treeHealth.ValueRW.Value -= treeDamageBufferElement.Value;
            }
            _treeDamageBuffer.Clear();
            
            _transform.ValueRW.Scale = _treeHealth.ValueRO.Value / _treeHealth.ValueRO.Max;
        }
    }
}