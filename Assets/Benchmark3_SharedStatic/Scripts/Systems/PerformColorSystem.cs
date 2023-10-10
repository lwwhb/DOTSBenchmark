using Benchmark3_SharedStatic.Scripts.Components;
using Benchmark3_SharedStatic.Scripts.SharedStaticData;
using Benchmark3_SharedStatic.Scripts.SystemGroups;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace Benchmark3_SharedStatic.Scripts.Systems
{
    [DisableAutoCreation]
    [UpdateInGroup(typeof(SharedStaticDataUpdateSystemGroup))]
    public partial struct PerformColorSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var entities = SharedCubesEntityColorMap.SharedValue.Data.entityColorMap.GetKeyArray(Allocator.Temp);
            foreach (var entity in entities)
            {
                float3 color = SharedCubesEntityColorMap.SharedValue.Data.entityColorMap[entity];
                SystemAPI.GetComponentRW<CubeColor>(entity).ValueRW.cubeColor = new float4(color, 1.0f);
            }
            entities.Dispose();
        }
    }
}