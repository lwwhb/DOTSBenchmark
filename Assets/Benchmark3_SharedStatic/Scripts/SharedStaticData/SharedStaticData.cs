using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace Benchmark3_SharedStatic.Scripts.SharedStaticData
{
    public struct GlobalSettings
    {
        public static readonly SharedStatic<GlobalSettings> SharedValue 
            = SharedStatic<GlobalSettings>.GetOrCreate<GlobalSettings>();
        public Random random;
        public int xHalfSize;
        public int yHalfSize;
    }

    public struct SharedCubesEntityColorMap
    {
        public static readonly SharedStatic<SharedCubesEntityColorMap> SharedValue 
            = SharedStatic<SharedCubesEntityColorMap>.GetOrCreate<SharedCubesEntityColorMap>();
        
        public SharedCubesEntityColorMap(int capacity)
        {
            entityColorMap = new NativeParallelHashMap<Entity, float3>(capacity, Allocator.Persistent);
        }
        
        public NativeParallelHashMap<Entity, float3> entityColorMap; 
    }
}