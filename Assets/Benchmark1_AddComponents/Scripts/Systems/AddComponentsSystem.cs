using DOTSBenchmark0;
using Unity.Burst;
using Unity.Entities;
using Unity.Rendering;

namespace DOTSBenchmark1
{
    [DisableAutoCreation]
    [RequireMatchingQueriesForUpdate]
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    [UpdateAfter(typeof(CubeGenerateSystem))]
    public partial struct AddComponentsSystem : ISystem, ISystemStartStop
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<CubeGenerator>();
        }
        public void OnStartRunning(ref SystemState state)
        {
            EntityQuery query = state.GetEntityQuery(typeof(MaterialMeshInfo));
            //Case 12 : add component by query
            //state.EntityManager.AddComponent<RotateSpeed>(query);
            
            //Case 13 : add enableable component by query;
            state.EntityManager.AddComponent<EnableableRotateSpeed>(query);

            state.Enabled = false;
        }
        public void OnStopRunning(ref SystemState state)
        {
        }
    }
}