using Unity.Burst;
using Unity.Entities;
using Unity.Rendering;

namespace DOTSBenchmark1
{
    [DisableAutoCreation]
    [RequireMatchingQueriesForUpdate]
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    [UpdateAfter(typeof(AddTagComponentsSystem))]
    public partial struct DisableTagComponentsSystem : ISystem, ISystemStartStop
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EnableableCubeTag>();
        }
        
        public void OnStartRunning(ref SystemState state)
        {
            state.EntityManager.SetComponentEnabled<EnableableCubeTag>(state.GetEntityQuery(typeof(EnableableCubeTag)),false);
            state.Enabled = false;
        }
        
        public void OnStopRunning(ref SystemState state)
        {
            
        }
    }
}