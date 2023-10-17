using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Scenes;
using UnityEngine;

namespace Benchmark4_ScenesLoad.Scripts.Systems
{
    [DisableAutoCreation]
    public partial struct SceneSectionsLoadSystem : ISystem
    {
        private float timer;
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            timer = 1.0f;
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var sectionEntities = SystemAPI.GetSingletonBuffer<ResolvedSectionEntity>();
            NativeArray<Entity> sectionEntitiesArray = CollectionHelper.CreateNativeArray<Entity>(sectionEntities.Length, Allocator.Temp);
            for (int i = 0; i < sectionEntities.Length; i++)
            {
                sectionEntitiesArray[i] = sectionEntities[i].SectionEntity;
            }

            timer -= SystemAPI.Time.DeltaTime;
            if (timer < 0)
            {
                for (int i = 0; i < sectionEntitiesArray.Length; i++)
                {
                    var sectionState = SceneSystem.GetSectionStreamingState(state.WorldUnmanaged, sectionEntitiesArray[i]);
                    if (sectionState == SceneSystem.SectionStreamingState.Loaded)
                    {
                        state.EntityManager.RemoveComponent<RequestSceneLoaded>(sectionEntitiesArray[i]);
                        if (i == sectionEntitiesArray.Length - 1)
                            timer = 1.0f;
                    }
                    else if (sectionState == SceneSystem.SectionStreamingState.Unloaded)
                    {
                        state.EntityManager.AddComponent<RequestSceneLoaded>(sectionEntitiesArray[i]);
                        if (i == sectionEntitiesArray.Length - 1)
                            timer = 1.0f;
                    }
                }
            }
            sectionEntitiesArray.Dispose();
        }
    }
}