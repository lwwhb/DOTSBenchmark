using Benchmark3_SharedStatic.Scripts.Jobs;
using Benchmark3_SharedStatic.Scripts.SharedStaticData;
using Benchmark3_SharedStatic.Scripts.SystemGroups;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Profiling;

namespace Benchmark3_SharedStatic.Scripts.Systems
{
    [DisableAutoCreation]
    [UpdateInGroup(typeof(SharedStaticDataUpdateSystemGroup))]
    public partial struct DyeGreenColorSystem : ISystem
    {
        private const int DyeGreenNumber = 10;

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            DyeGreenColor(ref state);
        }

        [BurstCompile]
        void DyeGreenColor(ref SystemState state)
        {
            var entities = SharedCubesEntityColorMap.SharedValue.Data.entityColorMap.GetKeyArray(Allocator.TempJob);
            int count = entities.Length;
            if(count < 1)
                return;
            DyeGreenColorJob job = new DyeGreenColorJob()
            {
                entities = entities,
                dyeGreenNumber = DyeGreenNumber,
                count = count
            };
            JobHandle jobHandle = job.Schedule();
            jobHandle.Complete();
            entities.Dispose();
        }
    }
}