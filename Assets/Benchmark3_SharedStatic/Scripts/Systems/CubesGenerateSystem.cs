using Benchmark3_SharedStatic.Scripts.Bakers;
using Benchmark3_SharedStatic.Scripts.Components;
using Benchmark3_SharedStatic.Scripts.SharedStaticData;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Scenes;
using Unity.Transforms;

namespace Benchmark3_SharedStatic.Scripts.Systems
{
    [DisableAutoCreation]
    [RequireMatchingQueriesForUpdate]
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    [UpdateAfter(typeof(SceneSystemGroup))]
    public partial struct CubesGenerateSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<CubeGenerator>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var generator = SystemAPI.GetSingleton<CubeGenerator>();
            var cubes = CollectionHelper.CreateNativeArray<Entity>(4 * GlobalSettings.SharedValue.Data.xHalfSize * GlobalSettings.SharedValue.Data.yHalfSize,
                Allocator.Temp);
            state.EntityManager.Instantiate(generator.cubeProtoType, cubes);

            int count = 0;
            foreach (var cube in cubes)
            {
                int x = count % (GlobalSettings.SharedValue.Data.xHalfSize * 2) - GlobalSettings.SharedValue.Data.xHalfSize;
                int y = count / (GlobalSettings.SharedValue.Data.xHalfSize * 2) - GlobalSettings.SharedValue.Data.yHalfSize;
                var position = new float3(x * 1.1f, y * 1.1f, 0);
                var transform = SystemAPI.GetComponentRW<LocalTransform>(cube);
                transform.ValueRW.Position = position;
                state.EntityManager.AddComponentData(cube,
                    new CubeColor { cubeColor = new float4(1.0f, 1.0f, 1.0f, 1.0f) });
                SharedCubesEntityColorMap.SharedValue.Data.entityColorMap.Add(cube,new float3(1.0f, 1.0f, 1.0f));
                count++;
            }
            cubes.Dispose();
            // 此System只在启动时运行一次，所以在第一次更新后关闭它。
            state.Enabled = false;

        }
    }
}