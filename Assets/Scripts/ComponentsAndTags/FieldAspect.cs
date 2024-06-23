using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace CPD.Gnoma
{
    public readonly partial struct FieldAspect : IAspect
    {
        public readonly Entity Entity;

        private readonly RefRO<LocalTransform> _transform;
        private LocalTransform Transform => _transform.ValueRO;

        private readonly RefRO<FieldProperties> _fieldProperties;
        private readonly RefRW<FieldRandom> _fieldRandom;
        private readonly RefRW<GnomaSpawnPoints> _gnomaSpawnPoints;
        private readonly RefRW<GnomaSpawnTimer> _gnomaSpawnTimer;

        public int NumberMushroomsToSpawn => _fieldProperties.ValueRO.NumberMushroomsToSpawn;
        public Entity MushroomPrefab => _fieldProperties.ValueRO.MushroomPrefab;

        public bool GnomaSpawnPointInitialized()
        {
            return _gnomaSpawnPoints.ValueRO.Value.IsCreated && GnomaSpawnPointCount > 0;
        }

        private int GnomaSpawnPointCount => _gnomaSpawnPoints.ValueRO.Value.Value.Value.Length;

        public LocalTransform GetRandomMushroomTransform()
        {
            return new LocalTransform
            {
                Position = GetRandomPosition(),
                Rotation = GetRandomRotation(),
                Scale = GetRandomScale(0.5f)
            };
        }

        private float3 GetRandomPosition()
        {
            float3 randomPosition;
            do
            {
                randomPosition = _fieldRandom.ValueRW.Value.NextFloat3(MinCorner, MaxCorner);
            } while (math.distancesq(Transform.Position, randomPosition) <= TREE_SAFETY_RADIUS_SQ);

            return randomPosition;
        }
        
        private float3 MinCorner => Transform.Position - HalfDimensions;
        private float3 MaxCorner => Transform.Position + HalfDimensions;
        private float3 HalfDimensions => new()
        {
            x = _fieldProperties.ValueRO.FieldDimensions.x * 0.5f,
            y = 0f,
            z = _fieldProperties.ValueRO.FieldDimensions.y * 0.5f
        };
        private const float TREE_SAFETY_RADIUS_SQ = 100;

        private quaternion GetRandomRotation() => quaternion.RotateY(_fieldRandom.ValueRW.Value.NextFloat(-0.25f, 0.25f));
        private float GetRandomScale(float min) => _fieldRandom.ValueRW.Value.NextFloat(min, 1f);

        public float2 GetRandomOffset()
        {
            return _fieldRandom.ValueRW.Value.NextFloat2();
        }

        public float GnomaSpawnTimer
        {
            get => _gnomaSpawnTimer.ValueRO.Value;
            set => _gnomaSpawnTimer.ValueRW.Value = value;
        }

        public bool TimeToSpawnGnoma => GnomaSpawnTimer <= 0f;

        public float GnomaSpawnRate => _fieldProperties.ValueRO.GnomaSpawnRate;

        public Entity GnomaPrefab => _fieldProperties.ValueRO.GnomaPrefab;

        public LocalTransform GetGnomaSpawnPoint()
        {
            var position = GetRandomGnomaSpawnPoint();
            return new LocalTransform
            {
                Position = position,
                Rotation = quaternion.RotateY(MathHelpers.GetHeading(position, Transform.Position)),
                Scale = 1f
            };
        }

        private float3 GetRandomGnomaSpawnPoint()
        {
            return GetGnomaSpawnPoint(_fieldRandom.ValueRW.Value.NextInt(GnomaSpawnPointCount));
        }

        private float3 GetGnomaSpawnPoint(int i) => _gnomaSpawnPoints.ValueRO.Value.Value.Value[i];

        public float3 Position => Transform.Position;


    }
}