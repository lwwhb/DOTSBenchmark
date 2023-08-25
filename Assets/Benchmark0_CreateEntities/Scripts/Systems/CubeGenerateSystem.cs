using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Scenes;
using Unity.Transforms;

namespace DOTSBenchmark0
{
    [DisableAutoCreation]
    [RequireMatchingQueriesForUpdate]
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    [UpdateAfter(typeof(SceneSystemGroup))]
    public partial struct CubeGenerateSystem : ISystem, ISystemStartStop
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<CubeGenerator>();
        }
        /*[BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var generator = SystemAPI.GetSingleton<CubeGenerator>();
            var cubes = CollectionHelper.CreateNativeArray<Entity>(4 * generator.halfCountX * generator.halfCountZ,
                Allocator.Temp);
            state.EntityManager.Instantiate(generator.cubeProtoType, cubes);

            int count = 0;
            foreach (var cube in cubes)
            {
                int x = count % (generator.halfCountX * 2) - generator.halfCountX;
                int z = count / (generator.halfCountX * 2) - generator.halfCountZ;
                var position = new float3(x * 1.1f, 0, z * 1.1f);
                var transform = SystemAPI.GetComponentRW<LocalTransform>(cube);
                transform.ValueRW.Position = position;
                count++;
            }

            cubes.Dispose();
            // 此System只在启动时运行一次，所以在第一次更新后关闭它。
            state.Enabled = false;
        }*/
        [BurstCompile]
        public void OnStartRunning(ref SystemState state)
        {
            var generator = SystemAPI.GetSingleton<CubeGenerator>();
            var cubes = CollectionHelper.CreateNativeArray<Entity>(4 * generator.halfCountX * generator.halfCountZ,
                Allocator.Temp);
            state.EntityManager.Instantiate(generator.cubeProtoType, cubes);

            int count = 0;
            foreach (var cube in cubes)
            {
                int x = count % (generator.halfCountX * 2) - generator.halfCountX;
                int z = count / (generator.halfCountX * 2) - generator.halfCountZ;
                var position = new float3(x * 1.1f, 0, z * 1.1f);
                var transform = SystemAPI.GetComponentRW<LocalTransform>(cube);
                transform.ValueRW.Position = position;
                count++;
            }

            cubes.Dispose();
            // 此System只在启动时运行一次，所以在第一次更新后关闭它。
            state.Enabled = false;
        }
        [BurstCompile]
        public void OnStopRunning(ref SystemState state)
        {
        }
    }
}
