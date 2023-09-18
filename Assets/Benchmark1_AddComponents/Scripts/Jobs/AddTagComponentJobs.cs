using Unity.Burst;
using Unity.Burst.Intrinsics;
using Unity.Entities;

namespace DOTSBenchmark1
{
    [BurstCompile]
    public partial struct AddTagComponentWithIJobChunk : IJobChunk
    {
        public EntityCommandBuffer.ParallelWriter writer;
        public EntityTypeHandle entityTypeHandle;

        public void Execute(in ArchetypeChunk chunk, int unfilteredChunkIndex, bool useEnabledMask, in v128 chunkEnabledMask)
        {
            writer.AddComponent<CubeTag>(unfilteredChunkIndex, chunk.GetNativeArray(entityTypeHandle));
        }
    }
    
    [BurstCompile]
    public partial struct AddTagComponentWithIJobEntity : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter writer;
        public void Execute(Entity entity, [ChunkIndexInQuery] int chunkIndexInQuery)
        {
            writer.AddComponent<CubeTag>(chunkIndexInQuery, entity);
        }
    }
}