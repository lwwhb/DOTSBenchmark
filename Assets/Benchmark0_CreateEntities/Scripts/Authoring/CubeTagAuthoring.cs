using Unity.Entities;
using UnityEngine;

namespace DOTSBenchmark0
{
    public struct CubeTag : IComponentData
    {
    }
    class CubeTagAuthoring : MonoBehaviour
    {
        class CubeTagBaker : Baker<CubeTagAuthoring>
        {
            public override void Bake(CubeTagAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new CubeTag());
            }
        }
    }
}