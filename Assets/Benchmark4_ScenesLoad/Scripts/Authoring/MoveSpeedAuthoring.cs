using Unity.Entities;
using UnityEngine;

namespace Benchmark4_ScenesLoad.Scripts.Authoring
{
    public struct MoveSpeed : IComponentData
    {
        public float speed;
    }
    public class MoveSpeedAuthoring : MonoBehaviour
    {
        [Range(1, 10)] public float moveSpeed = 1.0f;
        private class MoveSpeedAuthoringBaker : Baker<MoveSpeedAuthoring>
        {
            public override void Bake(MoveSpeedAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                var data = new MoveSpeed
                {
                    speed = authoring.moveSpeed
                };
                AddComponent(entity, data);
            }
        }
    }
}