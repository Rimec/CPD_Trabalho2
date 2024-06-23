using Unity.Entities;
using UnityEngine;

namespace CPD.Gnoma
{
    public class TreeMono : MonoBehaviour
    {
        public float TreeHealth;
    }

    public class TreeBaker : Baker<TreeMono>
    {
        public override void Bake(TreeMono authoring)
        {
            var treeEntity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<TreeTag>(treeEntity);
            AddComponent(treeEntity, new TreeHealth { Value = authoring.TreeHealth, Max = authoring.TreeHealth });
            AddBuffer<TreeDamageBufferElement>(treeEntity);
        }
    }
}