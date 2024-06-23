using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace CPD.Gnoma
{
    public class FieldMono : MonoBehaviour
    {
        public float2 FieldDimensions;
        public int NumberMushroomsToSpawn;
        public GameObject MushroomPrefab;
        public uint RandomSeed;
        public GameObject GnomaPrefab;
        public float GnomaSpawnRate;
        public class FieldBaker : Baker<FieldMono>
        {
            public override void Bake(FieldMono authoring)
            {
                Entity fieldEntity = GetEntity(TransformUsageFlags.Dynamic);
                FieldProperties fieldProperties = new()
                {
                    FieldDimensions = authoring.FieldDimensions,
                    NumberMushroomsToSpawn = authoring.NumberMushroomsToSpawn,
                    MushroomPrefab = GetEntity(authoring.MushroomPrefab, TransformUsageFlags.Dynamic),
                    GnomaPrefab = GetEntity(authoring.GnomaPrefab, TransformUsageFlags.Dynamic),
                    GnomaSpawnRate = authoring.GnomaSpawnRate
                };
                AddComponent(fieldEntity, fieldProperties);
                FieldRandom fieldRandom = new()
                {
                    Value = Unity.Mathematics.Random.CreateFromIndex(authoring.RandomSeed)
                };
                AddComponent(fieldEntity, fieldRandom);
                AddComponent<GnomaSpawnPoints>(fieldEntity);
                AddComponent<GnomaSpawnTimer>(fieldEntity);
            }
        }
    }

}
