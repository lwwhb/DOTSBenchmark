using Benchmark3_SharedStatic.Scripts.Components;
using Benchmark3_SharedStatic.Scripts.SharedStaticData;
using Benchmark3_SharedStatic.Scripts.SystemGroups;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Profiling;

namespace Benchmark3_SharedStatic.Scripts.Systems
{
    [DisableAutoCreation]
    //[RequireMatchingQueriesForUpdate]  // 没有Query的System，不需要这个，否则会不更新
    [UpdateInGroup(typeof(SharedStaticDataUpdateSystemGroup))]
    public partial struct DyeRedColorSystem : ISystem
    {
        private const int DyeRedNumber = 10;

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            DyeRedColor(ref state);
        }
        
        [BurstCompile]
        void DyeRedColor(ref SystemState stat)
        {
            var entities = SharedCubesEntityColorMap.SharedValue.Data.entityColorMap.GetKeyArray(Allocator.Temp);
            int count = entities.Length;
            if(count < 1)
                return;
            for (int i = 0; i < DyeRedNumber; i++)
            {
                int index = GlobalSettings.SharedValue.Data.random.NextInt(count);
                Entity entity = entities[index];
                float3 color = SharedCubesEntityColorMap.SharedValue.Data.entityColorMap[entity];
                if (color.Equals(new float3(1.0f, 1.0f, 1.0f)))
                    color = new float3(1.0f, 0.0f, 0.0f);
                else
                {
                    color += new float3(1.0f, 0.0f, 0.0f);
                    if (color.x > 1.0f)
                        color.x -= 1.0f;
                }
                SharedCubesEntityColorMap.SharedValue.Data.entityColorMap[entity] = color;
            }

            entities.Dispose();
        }
    }
}