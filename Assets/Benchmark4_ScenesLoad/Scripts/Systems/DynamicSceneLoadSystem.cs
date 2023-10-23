using Benchmark4_ScenesLoad.Scripts.Authoring;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Scenes;
using Unity.Transforms;
using UnityEngine;

namespace Benchmark4_ScenesLoad.Scripts.Systems
{
    [DisableAutoCreation]
    public partial struct DynamicSceneLoadSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<CustomMetadata>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            NativeHashSet<Entity> toLoad = new NativeHashSet<Entity>(1, Allocator.Temp);
            var subSceneQuery = SystemAPI.QueryBuilder().WithAll<CustomMetadata, SceneSection>().Build();
            var subSceneEntities = subSceneQuery.ToEntityArray(Allocator.Temp);
            var metadataArray = subSceneQuery.ToComponentDataArray<CustomMetadata>(Allocator.Temp);
            
            foreach (var transform in
                     SystemAPI.Query<RefRO<LocalTransform>>()
                         .WithAll<NextPathIndex>())
            {
                for (int index = 0; index < metadataArray.Length; ++index)
                {
                    float3 distance = transform.ValueRO.Position - metadataArray[index].position;
                    distance.z = 0;
                    float radiusSq = metadataArray[index].radius;
                    
                    Color debugColor = new Color(1f, 0f, 0f);
                    if (math.lengthsq(distance) < radiusSq * radiusSq)
                    {
                        toLoad.Add(subSceneEntities[index]);
                        debugColor = new Color(0f, 0.5f, 0f);
                    }

                    DrawDebugMetadata(metadataArray[index].position - new float3(0f, 0, 0.2f),
                        metadataArray[index].radius, debugColor);
                }
            }
            
            //根据判定结果加载或卸载场景
            foreach (Entity subsceneEntity in subSceneEntities)
            {
                if (toLoad.Contains(subsceneEntity))
                {
                    if (!SceneSystem.IsSceneLoaded(state.WorldUnmanaged, subsceneEntity))
                    {
                        SceneSystem.LoadSceneAsync(state.WorldUnmanaged, subsceneEntity);
                    }
                }
                else
                {
                    if (SceneSystem.IsSceneLoaded(state.WorldUnmanaged, subsceneEntity))
                    {
                        var sceneStreamingState = SceneSystem.GetSceneStreamingState(state.WorldUnmanaged, subsceneEntity);
                        if (sceneStreamingState == SceneSystem.SceneStreamingState.LoadedSuccessfully)
                        {
                            SceneSystem.UnloadScene(state.WorldUnmanaged, subsceneEntity);
                        }
                    }
                }
            }
        }

        public static void DrawDebugMetadata(float3 position, float radius, Color color, float divisions = 8f)
        {
            float angle = 0f;
            float step = math.PI / divisions;
            float PI2 = math.PI * 2f;
            while (angle < PI2)
            {
                float3 begin = new float3(math.sin(angle), math.cos(angle), 0f) * radius + position;
                angle += step;
                float3 end = new float3(math.sin(angle), math.cos(angle),0f ) * radius + position;
                Debug.DrawLine(begin, end, color);
            }
        }
    }
}