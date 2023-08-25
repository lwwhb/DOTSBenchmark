using Unity.Entities;
using UnityEngine;

namespace DOTSBenchmark0
{
    struct CubeGenerator : IComponentData
    {
        public Entity cubeProtoType;
        public int halfCountX;
        public int halfCountZ;
    }

    class CubeGeneratorAuthoring : MonoBehaviour
    {
        public GameObject cubePrefab = null;
        [Range(10, 100)] public int xHalfCount = 40;
        [Range(10, 100)] public int zHalfCount = 40;

        class CubeBaker : Baker<CubeGeneratorAuthoring>
        {
            public override void Bake(CubeGeneratorAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new CubeGenerator
                {
                    cubeProtoType = GetEntity(authoring.cubePrefab, TransformUsageFlags.Dynamic),
                    halfCountX = authoring.xHalfCount,
                    halfCountZ = authoring.zHalfCount
                });
            }
        }
    }
}
