using Unity.Burst;
using Unity.Entities;

namespace DOTSBenchmark1
{
    [DisableAutoCreation]
    [RequireMatchingQueriesForUpdate]
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    [UpdateAfter(typeof(AddComponentsSystem))]
    public partial struct DisableComponentsSystem : ISystem, ISystemStartStop
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EnableableRotateSpeed>();
        }
        
        public void OnStartRunning(ref SystemState state)
        {
            state.EntityManager.SetComponentEnabled<EnableableRotateSpeed>(state.GetEntityQuery(typeof(EnableableRotateSpeed)),false);
            state.Enabled = false;
        }
        
        public void OnStopRunning(ref SystemState state)
        {
        }
    }
}