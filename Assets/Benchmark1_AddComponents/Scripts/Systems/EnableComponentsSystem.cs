using Unity.Burst;
using Unity.Entities;

namespace DOTSBenchmark1
{
    [DisableAutoCreation]
    [RequireMatchingQueriesForUpdate]
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    [UpdateAfter(typeof(DisableComponentsSystem))]
    public partial struct EnableComponentsSystem : ISystem, ISystemStartStop
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EnableableRotateSpeed>();
        }
        
        public void OnStartRunning(ref SystemState state)
        {
            //Case 15 : enable enableable component by query;
            state.EntityManager.SetComponentEnabled<EnableableRotateSpeed>(state.GetEntityQuery(typeof(EnableableRotateSpeed)),true);
            state.Enabled = false;
        }
        
        public void OnStopRunning(ref SystemState state)
        {
        }
    }
}