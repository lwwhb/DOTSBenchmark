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
    public partial class UIInteropManagedSystem : SystemBase
    {
        private UIEventHandler _handler;
        public void SetUIEventHandler(UIEventHandler handler)
        {
            _handler = handler;
        }
        protected override void OnCreate()
        {
            RequireForUpdate<AssetsReferences>();
            EntityManager.AddComponentData(this.SystemHandle, new LoadedEntityAssets() { entity = Entity.Null, entityInstance = Entity.Null});
            EntityManager.AddComponentData(this.SystemHandle, new LoadedGoAssets(){ gameObject = null, gameObjectInstance = null});
        }

        protected override void OnUpdate()
        {
            var assetsRef = SystemAPI.GetSingleton<AssetsReferences>();
            if (assetsRef.entityPrefabReference.IsReferenceValid)
            {
                LoadedEntityAssets asset = EntityManager.GetComponentData<LoadedEntityAssets>(this.SystemHandle);
                if (asset.entity != Entity.Null &&EntityManager.HasComponent<PrefabLoadResult>(asset.entity))
                {
                    var data = EntityManager.GetComponentData<PrefabLoadResult>(asset.entity);
                    if (data.PrefabRoot != Entity.Null)
                    {
                        EntityManager.DestroyEntity(asset.entity);
                        asset.entity = data.PrefabRoot;
                        if (asset.entityInstance == Entity.Null)
                            asset.entityInstance = EntityManager.Instantiate(asset.entity);
                        EntityManager.SetComponentData<LoadedEntityAssets>(this.SystemHandle, asset);
                        if (_handler != null)
                            _handler.OnEntityPrefabLoaded();
                    }
                }
            }
            if (assetsRef.gameObjectPrefabReference.IsReferenceValid)
            {
                if(assetsRef.gameObjectPrefabReference.LoadingStatus == ObjectLoadingStatus.Completed)
                {
                    LoadedGoAssets asset = EntityManager.GetComponentData<LoadedGoAssets>(this.SystemHandle);
                    if (asset != null && asset.gameObject == null)
                    {
                        asset.gameObject = assetsRef.gameObjectPrefabReference.Result;
                        if(asset.gameObjectInstance == null)
                            asset.gameObjectInstance = Object.Instantiate(asset.gameObject, new Vector3(0, -3, 0), Quaternion.identity);
                        if (_handler != null)
                            _handler.OnGameObjectPrefabLoaded();
                    }
                    assetsRef.gameObjectPrefabReference.Release();  //弱引用要手动释放
                }
            }
        }

        public void LoadEntityPrefab()
        {
            var assetsRef = SystemAPI.GetSingleton<AssetsReferences>();
            if (assetsRef.entityPrefabReference.IsReferenceValid)
            {
                LoadedEntityAssets asset = EntityManager.GetComponentData<LoadedEntityAssets>(this.SystemHandle);
                if(asset.entity == Entity.Null)
                {
                    asset.entity = EntityManager.CreateEntity();
                    EntityManager.AddComponentData<RequestEntityPrefabLoaded>(asset.entity, new RequestEntityPrefabLoaded{
                        Prefab = assetsRef.entityPrefabReference
                    });
                    EntityManager.SetComponentData<LoadedEntityAssets>(this.SystemHandle, asset);
                }
            }
        }

        public void UnloadEntityPrefab()
        {
            var assetsRef = SystemAPI.GetSingleton<AssetsReferences>();
            if (assetsRef.entityPrefabReference.IsReferenceValid)
            {
                LoadedEntityAssets asset = EntityManager.GetComponentData<LoadedEntityAssets>(this.SystemHandle);
                if(asset.entity != Entity.Null)
                {
                    if (asset.entityInstance != Entity.Null)
                    {
                        EntityManager.DestroyEntity(asset.entityInstance);
                        asset.entityInstance = Entity.Null;
                    }
                    EntityManager.DestroyEntity(asset.entity);
                    asset.entity = Entity.Null;
                    EntityManager.SetComponentData<LoadedEntityAssets>(this.SystemHandle, asset);
                    if (_handler != null)
                        _handler.OnEntityPrefabUnloaded();
                }
            }
        }

        public void LoadGoPrefab()
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
                LoadedGoAssets asset = EntityManager.GetComponentData<LoadedGoAssets>(this.SystemHandle);
                if (asset.gameObjectInstance != null)
                {
                    Object.Destroy(asset.gameObjectInstance);
                    asset.gameObjectInstance = null;
                    asset.gameObject = null;
                }
                assetsRef.gameObjectPrefabReference.Release();
                if (_handler != null)
                    _handler.OnGameObjectPrefabUnloaded();
            }
        }
        
    }
}