using DOTSBenchmark0;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Rendering;

namespace DOTSBenchmark1
{
    [DisableAutoCreation]
    [RequireMatchingQueriesForUpdate]
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    [UpdateAfter(typeof(CubeGenerateSystem))]
    public partial struct AddTagComponentsSystem : ISystem, ISystemStartStop
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<CubeGenerator>();
            
        }
        
        public void OnStartRunning(ref SystemState state)
        {
            //Case 1 : add tag component by query
            //state.EntityManager.AddComponent<CubeTag>(state.GetEntityQuery(typeof(MaterialMeshInfo)));

            //Case 2 : add tag component by entity array
            //NativeArray<Entity> entities = state.GetEntityQuery(typeof(MaterialMeshInfo)).ToEntityArray(Allocator.Temp);
            //state.EntityManager.AddComponent<CubeTag>(entities);
            
            //Case 3 : add tag component by entity foreach
            /*NativeArray<Entity> entities = state.GetEntityQuery(typeof(MaterialMeshInfo)).ToEntityArray(Allocator.Temp);
            foreach (var entity in entities)
            {
                state.EntityManager.AddComponent<CubeTag>(entity);
            }*/
            
            //Case 4 : add tag component by ecb + query
            /*EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
            ecb.AddComponent<CubeTag>(state.GetEntityQuery(typeof(MaterialMeshInfo)), new CubeTag());
            ecb.Playback(state.EntityManager);
            ecb.Dispose();*/
            
            //Case 5 : add tag component by ecb + entity array
            /*EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
            NativeArray<Entity> entities = state.GetEntityQuery(typeof(MaterialMeshInfo)).ToEntityArray(Allocator.Temp);
            ecb.AddComponent<CubeTag>(entities, new CubeTag());
            ecb.Playback(state.EntityManager);
            ecb.Dispose();*/
            
            //Case 6 : add tag component by ecb + entity foreach
            /*EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.Temp);
            NativeArray<Entity> entities = state.GetEntityQuery(typeof(MaterialMeshInfo)).ToEntityArray(Allocator.Temp);
            foreach (var entity in entities)
            {
                ecb.AddComponent<CubeTag>(entity, new CubeTag());
            }
            ecb.Playback(state.EntityManager);
            ecb.Dispose();*/
            
            //Case 7 : add tag component by ecb + IJobChunk parallel
            /*EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob);
            var job = new AddTagComponentWithIJobChunk()
            {
                writer = ecb.AsParallelWriter(),
                entityTypeHandle = state.EntityManager.GetEntityTypeHandle()
            };
            state.Dependency = job.ScheduleParallel(state.GetEntityQuery(typeof(MaterialMeshInfo)), state.Dependency);
            state.Dependency.Complete();
            ecb.Playback(state.EntityManager);
            ecb.Dispose();*/
            
            //Case 8 : add tag component by ecb + IJobEntity parallel
            /*EntityCommandBuffer ecb = new EntityCommandBuffer(Allocator.TempJob);
            var job = new AddTagComponentWithIJobEntity()
            {
                writer = ecb.AsParallelWriter()
            };
            state.Dependency = job.ScheduleParallel(state.GetEntityQuery(typeof(MaterialMeshInfo)), state.Dependency);
            state.Dependency.Complete();
            ecb.Playback(state.EntityManager);
            ecb.Dispose();*/
            
            //case 9 : add enableable tag component by query
            state.EntityManager.AddComponent<EnableableCubeTag>(state.GetEntityQuery(typeof(MaterialMeshInfo)));

            //case 10 : add enableable tag component after add tag component
            //state.EntityManager.AddComponent<CubeTag>(state.GetEntityQuery(typeof(MaterialMeshInfo)));
            //state.EntityManager.AddComponent<EnableableCubeTag>(state.GetEntityQuery(typeof(MaterialMeshInfo)));
            
            //case 11 : add enableable tag component before add tag component
            //state.EntityManager.AddComponent<EnableableCubeTag>(state.GetEntityQuery(typeof(MaterialMeshInfo)));
            //state.EntityManager.AddComponent<CubeTag>(state.GetEntityQuery(typeof(MaterialMeshInfo)));

            state.Enabled = false;
        }
        
        public void OnStopRunning(ref SystemState state)
        {
        }
    }
}