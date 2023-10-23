using Benchmark4_ScenesLoad.Scripts.Authoring;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Benchmark4_ScenesLoad.Scripts.Systems
{
    [DisableAutoCreation]
    public partial struct PathFindingSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<WayPoint>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            DynamicBuffer<WayPoint> path = SystemAPI.GetSingletonBuffer<WayPoint>();
            float deltaTime = SystemAPI.Time.DeltaTime;
            if (!path.IsEmpty)
            {
                foreach (var (transform, nextIndex, speed) in
                         SystemAPI.Query<RefRW<LocalTransform>, RefRW<NextPathIndex>, RefRO<MoveSpeed>>())
                {
                    float3 direction = path[(int)nextIndex.ValueRO.nextIndex].point - transform.ValueRO.Position;
                    transform.ValueRW.Position =
                        transform.ValueRO.Position + math.normalize(direction) * speed.ValueRO.speed*deltaTime;
                    if (math.distance(path[(int)nextIndex.ValueRO.nextIndex].point, transform.ValueRO.Position) <=
                        0.02f)
                    {
                        nextIndex.ValueRW.nextIndex = (uint)((nextIndex.ValueRO.nextIndex + 1) % path.Length);
                    }
                }

                DrawDebugPath(path);
            }
        }
        
        public static void DrawDebugPath(DynamicBuffer<WayPoint> path)
        {
            for (int i = 0; i < path.Length; i++)
            {
                Debug.DrawLine(path[i].point - new float3(0,0, 1), path[(i+1)%path.Length].point - new float3(0,0, 1), Color.yellow);
            }
        }
    }
}