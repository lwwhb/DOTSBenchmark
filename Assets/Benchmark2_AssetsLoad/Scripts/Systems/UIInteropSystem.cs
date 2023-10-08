using Benchmark2_AssetsLoad.Scripts.Bakers;
using Benchmark2_AssetsLoad.Scripts.Components;
using Benchmark2_AssetsLoad.Scripts.MonoBehaviours;
using Unity.Entities;
using Unity.Entities.Content;
using Unity.Scenes;
using UnityEngine;

namespace Benchmark2_AssetsLoad.Scripts.Systems
{
    [DisableAutoCreation]
    [RequireMatchingQueriesForUpdate]
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    [UpdateAfter(typeof(SceneSystemGroup))]
    public partial struct UIInteropSystem : ISystem, ISystemStartStop
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<AssetsReferences>();
            state.EntityManager.AddComponentData(state.SystemHandle, new LoadedEntityAssets() { entity = Entity.Null, entityInstance = Entity.Null });
            state.EntityManager.AddComponentData(state.SystemHandle, new LoadedGoAssets(){ gameObject = null, gameObjectInstance = null });
            state.EntityManager.AddComponentData(state.SystemHandle, new UIEventBridge()
            {
                handler = Object.FindObjectOfType<UIEventHandler>()
            });
        }
        
        public void OnUpdate(ref SystemState state)
        {
            var assetsRef = SystemAPI.GetSingleton<AssetsReferences>();
            if (assetsRef.entityPrefabReference.IsReferenceValid)
            {
                LoadedEntityAssets asset = state.EntityManager.GetComponentData<LoadedEntityAssets>(state.SystemHandle);
                if (asset.entity != Entity.Null && state.EntityManager.HasComponent<PrefabLoadResult>(asset.entity))
                {
                    var data = state.EntityManager.GetComponentData<PrefabLoadResult>(asset.entity);
                    if (data.PrefabRoot != Entity.Null)
                    {
                        state.EntityManager.DestroyEntity(asset.entity);
                        asset.entity = data.PrefabRoot;
                        if (asset.entityInstance == Entity.Null)
                            asset.entityInstance = state.EntityManager.Instantiate(asset.entity);
                        state.EntityManager.SetComponentData<LoadedEntityAssets>(state.SystemHandle, asset);

                        var uiEventBridge = state.EntityManager.GetComponentData<UIEventBridge>(state.SystemHandle);
                        if (uiEventBridge.handler != null)
                            uiEventBridge.handler.OnEntityPrefabLoaded();
                    }
                }
            }
            if (assetsRef.gameObjectPrefabReference.IsReferenceValid)
            {
                if(assetsRef.gameObjectPrefabReference.LoadingStatus == ObjectLoadingStatus.Completed)
                {
                    LoadedGoAssets asset = state.EntityManager.GetComponentData<LoadedGoAssets>(state.SystemHandle);
                    if (asset != null && asset.gameObject == null)
                    {
                        asset.gameObject = assetsRef.gameObjectPrefabReference.Result;
                        if(asset.gameObjectInstance == null)
                            asset.gameObjectInstance = Object.Instantiate(asset.gameObject, new Vector3(0, -3, 0), Quaternion.identity);
                        var uiEventBridge = state.EntityManager.GetComponentData<UIEventBridge>(state.SystemHandle);
                        if (uiEventBridge.handler != null)
                            uiEventBridge.handler.OnGameObjectPrefabLoaded();
                    }
                    assetsRef.gameObjectPrefabReference.Release(); //弱引用要手动释放
                }
            }
        }

        public void OnStartRunning(ref SystemState state)
        {
#if USE_UNMANAGEDSYSTEM
            var eventBridge = state.EntityManager.GetComponentData<UIEventBridge>(state.SystemHandle);
            if ( eventBridge.handler != null)
            {
                eventBridge.handler.OnLoadEntityPrefab += LoadEntityPrefab;
                eventBridge.handler.OnLoadGameObjectPrefab += LoadGoPrefab;
                eventBridge.handler.OnUnloadEntityPrefab += UnloadEntityPrefab;
                eventBridge.handler.OnUnloadGameObjectPrefab += UnloadGoPrefab;
            }
#endif
        }

        public void OnStopRunning(ref SystemState state)
        {
#if USE_UNMANAGEDSYSTEM
            var eventBridge = state.EntityManager.GetComponentData<UIEventBridge>(state.SystemHandle);
            if ( eventBridge.handler != null)
            {
                eventBridge.handler.OnLoadEntityPrefab -= LoadEntityPrefab;
                eventBridge.handler.OnLoadGameObjectPrefab -= LoadGoPrefab;
                eventBridge.handler.OnUnloadEntityPrefab -= UnloadEntityPrefab;
                eventBridge.handler.OnUnloadGameObjectPrefab -= UnloadGoPrefab;
            }
#endif
        }

        private void LoadEntityPrefab()
        {
            var assetsRef = SystemAPI.GetSingleton<AssetsReferences>();
            if (assetsRef.entityPrefabReference.IsReferenceValid)
            {
                var systemHandle = World.DefaultGameObjectInjectionWorld.GetExistingSystem<UIInteropSystem>();
                LoadedEntityAssets asset = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<LoadedEntityAssets>(systemHandle);
                if(asset.entity == Entity.Null)
                {
                    asset.entity = World.DefaultGameObjectInjectionWorld.EntityManager.CreateEntity();
                    World.DefaultGameObjectInjectionWorld.EntityManager.AddComponentData<RequestEntityPrefabLoaded>(asset.entity, new RequestEntityPrefabLoaded{
                        Prefab = assetsRef.entityPrefabReference
                    });
                    World.DefaultGameObjectInjectionWorld.EntityManager.SetComponentData<LoadedEntityAssets>(systemHandle, asset);
                }
            }
        }

        public void UnloadEntityPrefab()
        {
            var assetsRef = SystemAPI.GetSingleton<AssetsReferences>();
            if (assetsRef.entityPrefabReference.IsReferenceValid)
            {
                var systemHandle = World.DefaultGameObjectInjectionWorld.GetExistingSystem<UIInteropSystem>();
                LoadedEntityAssets asset = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<LoadedEntityAssets>(systemHandle);
                if(asset.entity != Entity.Null)
                {
                    if (asset.entityInstance != Entity.Null)
                    {
                        World.DefaultGameObjectInjectionWorld.EntityManager.DestroyEntity(asset.entityInstance);
                        asset.entityInstance = Entity.Null;
                    }
                    World.DefaultGameObjectInjectionWorld.EntityManager.DestroyEntity(asset.entity);
                    asset.entity = Entity.Null;
                    World.DefaultGameObjectInjectionWorld.EntityManager.SetComponentData<LoadedEntityAssets>(systemHandle, asset);
                    
                    var uiEventBridge = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<UIEventBridge>(systemHandle);
                    if (uiEventBridge.handler != null)
                        uiEventBridge.handler.OnEntityPrefabUnloaded();
                }
            }
        }
        
        private void LoadGoPrefab()
        {
            var assetsRef = SystemAPI.GetSingleton<AssetsReferences>();
            if (assetsRef.gameObjectPrefabReference.IsReferenceValid)
            {
                if (assetsRef.gameObjectPrefabReference.LoadingStatus == ObjectLoadingStatus.None)
                {
                    assetsRef.gameObjectPrefabReference.LoadAsync();
                }
            }
        }
        
        public void UnloadGoPrefab()
        {
            var assetsRef = SystemAPI.GetSingleton<AssetsReferences>();
            if (assetsRef.gameObjectPrefabReference.IsReferenceValid)
            {
                var systemHandle = World.DefaultGameObjectInjectionWorld.GetExistingSystem<UIInteropSystem>();
                LoadedGoAssets asset = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<LoadedGoAssets>(systemHandle);
                if (asset.gameObjectInstance != null)
                {
                    Object.Destroy(asset.gameObjectInstance);
                    asset.gameObjectInstance = null;
                    asset.gameObject = null;
                }
                assetsRef.gameObjectPrefabReference.Release();
                var uiEventBridge = World.DefaultGameObjectInjectionWorld.EntityManager.GetComponentData<UIEventBridge>(systemHandle);
                if (uiEventBridge.handler != null)
                    uiEventBridge.handler.OnGameObjectPrefabUnloaded();
            }
        }
    }
}