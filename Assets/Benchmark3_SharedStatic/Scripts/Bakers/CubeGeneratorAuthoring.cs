using Unity.Entities;
using UnityEngine;

namespace Benchmark3_SharedStatic.Scripts.Bakers
{
    public struct CubeGenerator : IComponentData
    {
        public Entity cubeProtoType;
    }

    public class CubeGeneratorAuthoring : MonoBehaviour
    {
        public GameObject cubePrefab = null;
        class CubeBaker : Baker<CubeGeneratorAuthoring>
        {
            public override void Bake(CubeGeneratorAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new CubeGenerator
                {
                    cubeProtoType = GetEntity(authoring.cubePrefab, TransformUsageFlags.Dynamic),
                });
            }
        }
    }
}