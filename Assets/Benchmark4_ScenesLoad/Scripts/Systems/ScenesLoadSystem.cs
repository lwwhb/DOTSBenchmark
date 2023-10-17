using Benchmark4_ScenesLoad.Scripts.Authoring;
using Unity.Burst;
using Unity.Entities;
using Unity.Scenes;
using UnityEngine;

namespace Benchmark4_ScenesLoad.Scripts.Systems
{
    [DisableAutoCreation]
    public partial struct ScenesLoadSystem : ISystem, ISystemStartStop
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<SubscenesReferences>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var references = SystemAPI.GetSingletonRW<SubscenesReferences>();
            SceneSystem.SceneStreamingState ssstate = SceneSystem.GetSceneStreamingState(state.WorldUnmanaged, references.ValueRO.cubeSceneMetaEntity);
            if (ssstate == SceneSystem.SceneStreamingState.LoadedSuccessfully)
            {
                //问题2
                //state.EntityManager.DestroyEntity(references.ValueRW.cubeSceneMetaEntity);
                //SceneSystem.UnloadScene(state.WorldUnmanaged, references.ValueRW.cubeSceneMetaEntity);
                //SceneSystem.UnloadScene(state.WorldUnmanaged, references.ValueRW.cubeSceneMetaEntity, SceneSystem.UnloadParameters.DestroyMetaEntities);
            }
            
            //问题1
            ssstate = SceneSystem.GetSceneStreamingState(state.WorldUnmanaged, references.ValueRO.sphereSceneMetaEntity);
            if (ssstate == SceneSystem.SceneStreamingState.LoadedSectionEntities)
            {
                SceneSystem.LoadSceneAsync(state.WorldUnmanaged, references.ValueRW.sphereSceneMetaEntity);
            }
        }

        public void OnStartRunning(ref SystemState state)
        {
            var references = SystemAPI.GetSingletonRW<SubscenesReferences>();
            if (references.ValueRO.cubeSceneReference.IsReferenceValid)
            {
                references.ValueRW.cubeSceneMetaEntity =
                    SceneSystem.LoadSceneAsync(state.WorldUnmanaged, references.ValueRO.cubeSceneReference);
            }
            if (references.ValueRO.sphereSceneReference.IsReferenceValid)
            {
                references.ValueRW.sphereSceneMetaEntity =
                    SceneSystem.LoadSceneAsync(state.WorldUnmanaged, references.ValueRO.sphereSceneReference, new SceneSystem.LoadParameters
                    {
                        AutoLoad = false
                    });
            }

            if (references.ValueRO.capsuleSceneReference.IsReferenceValid)
            {
                references.ValueRW.capsuleSceneMetaEntity =
                    SceneSystem.LoadSceneAsync(state.WorldUnmanaged, references.ValueRO.capsuleSceneReference);
            }

            if (references.ValueRO.cylinderSceneReference.IsReferenceValid)
            {
                references.ValueRW.cylinderSceneMetaEntity = SceneSystem.LoadSceneAsync(state.WorldUnmanaged,
                    references.ValueRO.cylinderSceneReference);
            }
        }

        public void OnStopRunning(ref SystemState state)
        {
        }
    }
}