using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Scenes;

namespace DOTSBenchmark0
{
    [DisableAutoCreation]
    [RequireMatchingQueriesForUpdate]
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    [UpdateAfter(typeof(SceneSystemGroup))]
    public partial struct CubeGenerateWithJobSystem : ISystem, ISystemStartStop
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<CubeGenerator>();
        }

        public void OnStartRunning(ref SystemState state)
        {
            var generator = SystemAPI.GetSingleton<CubeGenerator>();
            EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob);
            var cubes = CollectionHelper.CreateNativeArray<Entity>(4 * generator.halfCountX * generator.halfCountZ, Allocator.TempJob);
            EntityCommandBuffer.ParallelWriter ecbParallel = ecb.AsParallelWriter();
            var job = new CubesGenerateJob()
            {
                cubeProtoType = generator.cubeProtoType,
                cubes = cubes,
                halfCountX = generator.halfCountX,
                halfCountZ = generator.halfCountZ,
                ecbParallel = ecbParallel
            };
            state.Dependency = job.ScheduleParallel(cubes.Length, 1, state.Dependency);
            state.Dependency.Complete();
            ecb.Playback(state.EntityManager);
            cubes.Dispose();
            ecb.Dispose();
        }

        public void OnStopRunning(ref SystemState state)
        {
            
        }
    }
}