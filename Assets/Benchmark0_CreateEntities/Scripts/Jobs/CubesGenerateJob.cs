using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

namespace DOTSBenchmark0
{
    [BurstCompile]
    struct CubesGenerateJob : IJobFor
    {
        [ReadOnly] public Entity cubeProtoType;
        public int halfCountX;
        public int halfCountZ;
        public NativeArray<Entity> cubes;
        public EntityCommandBuffer.ParallelWriter ecbParallel;
        public void Execute(int index)
        {
            cubes[index] = ecbParallel.Instantiate(index, cubeProtoType);
            int x = index % (halfCountX * 2) - halfCountX;
            int z = index / (halfCountX * 2) - halfCountZ;
            var position = new float3(x * 1.1f, 0, z * 1.1f);
            ecbParallel.SetComponent(index, cubes[index], new LocalTransform
            {
                Position = position,
                Rotation = quaternion.identity,
                Scale = 1
            });
        }
    }
}