using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Benchmark4_ScenesLoad.Scripts.Authoring
{
    public struct CustomMetadata : IComponentData
    {
        public float3 position; //角色位置
        public float radius;    //加载半径
    }
    public class CustomMetadataAuthoring : MonoBehaviour
    {
        [Range(1, 10)] public float radius = 1.0f;
        private class CustomMetadataAuthoringBaker : Baker<CustomMetadataAuthoring>
        {
            public override void Bake(CustomMetadataAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new CustomMetadata
                {
                    radius = authoring.radius,
                    position = GetComponent<Transform>().position
                });
            }
        }
    }
}