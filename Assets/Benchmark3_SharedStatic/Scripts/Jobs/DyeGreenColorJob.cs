using Benchmark3_SharedStatic.Scripts.SharedStaticData;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

namespace Benchmark3_SharedStatic.Scripts.Jobs
{
    [BurstCompile]
    public partial struct DyeGreenColorJob : IJob
    {
        public NativeArray<Entity> entities;
        public int dyeGreenNumber;
        public int count;
        public void Execute()
        {
            for (int i = 0; i < dyeGreenNumber; i++)
            {
                int rand = GlobalSettings.SharedValue.Data.random.NextInt(count);
                Entity entity = entities[rand];
                float3 color = SharedCubesEntityColorMap.SharedValue.Data.entityColorMap[entity];
                if (color.Equals(new float3(1.0f, 1.0f, 1.0f)))
                    color = new float3(0.0f, 1.0f, 0.0f);
                else
                {
                    color += new float3(0.0f, 1.0f, 0.0f);
                    if (color.y > 1.0f)
                        color.y -= 1.0f;
                }
                SharedCubesEntityColorMap.SharedValue.Data.entityColorMap[entity] = color;
            }
        }
    }
}