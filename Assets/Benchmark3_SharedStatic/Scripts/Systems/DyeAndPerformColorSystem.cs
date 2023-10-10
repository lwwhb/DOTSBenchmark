using Benchmark3_SharedStatic.Scripts.Components;
using Benchmark3_SharedStatic.Scripts.SharedStaticData;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

namespace Benchmark3_SharedStatic.Scripts.Systems
{
    [DisableAutoCreation]
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public partial struct DyeAndPerformColorSystem : ISystem
    {
        private const int DyeRedNumber = 10;
        private const int DyeGreenNumber = 10;
        private const int DyeBlueNumber = 10;

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            int cubeCount = GlobalSettings.SharedValue.Data.xHalfSize * GlobalSettings.SharedValue.Data.yHalfSize * 4;
            int counter = 0;
            foreach (var (color, entity) in SystemAPI.Query<RefRW<CubeColor>>().WithEntityAccess())
            {
                for (int i = 0; i < DyeRedNumber; i++)
                {
                    int index = GlobalSettings.SharedValue.Data.random.NextInt(cubeCount);
                    if (counter == index)
                    {
                        if (color.ValueRO.cubeColor.Equals(new float4(1.0f, 1.0f, 1.0f, 1.0f)))
                            color.ValueRW.cubeColor = new float4(1.0f, 0.0f, 0.0f, 1.0f);
                        else
                        {
                            color.ValueRW.cubeColor.xyz += new float3(1.0f, 0.0f, 0.0f);
                            if (color.ValueRW.cubeColor.x > 1.0f)
                                color.ValueRW.cubeColor.x -= 1.0f;
                        }
                    }
                }
                
                for (int i = 0; i < DyeGreenNumber; i++)
                {
                    int index = GlobalSettings.SharedValue.Data.random.NextInt(cubeCount);
                    if (counter == index)
                    {
                        if (color.ValueRO.cubeColor.Equals(new float4(1.0f, 1.0f, 1.0f, 1.0f)))
                            color.ValueRW.cubeColor = new float4(0.0f, 1.0f, 0.0f, 1.0f);
                        else
                        {
                            color.ValueRW.cubeColor.xyz += new float3(0.0f, 1.0f, 0.0f);
                            if (color.ValueRW.cubeColor.y > 1.0f)
                                color.ValueRW.cubeColor.y -= 1.0f;
                        }
                    }
                }
                
                for (int i = 0; i < DyeBlueNumber; i++)
                {
                    int index = GlobalSettings.SharedValue.Data.random.NextInt(cubeCount);
                    if (counter == index)
                    {
                        if (color.ValueRO.cubeColor.Equals(new float4(1.0f, 1.0f, 1.0f, 1.0f)))
                            color.ValueRW.cubeColor = new float4(0.0f, 0.0f, 1.0f, 1.0f);
                        else
                        {
                            color.ValueRW.cubeColor.xyz += new float3(0.0f, 0.0f, 1.0f);
                            if (color.ValueRW.cubeColor.z > 1.0f)
                                color.ValueRW.cubeColor.z -= 1.0f;
                        }
                    }
                }
                counter++;
            }
        }
    }
}