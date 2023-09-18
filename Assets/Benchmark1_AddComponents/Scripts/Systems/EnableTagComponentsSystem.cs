using Unity.Burst;
using Unity.Entities;

namespace DOTSBenchmark1
{
    [DisableAutoCreation]
    [RequireMatchingQueriesForUpdate]
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    [UpdateAfter(typeof(DisableTagComponentsSystem))]
    public partial struct EnableTagComponentsSystem : ISystem, ISystemStartStop
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EnableableCubeTag>();
        }
        
        public void OnStartRunning(ref SystemState state)
        {
            //Case 14 : enable enableable tag component by query;
            state.EntityManager.SetComponentEnabled<EnableableCubeTag>(state.GetEntityQuery(typeof(EnableableCubeTag)),true);
            state.Enabled = false;
        }
        
        public void OnStopRunning(ref SystemState state)
        {
        }
    }
}