using System.Collections.Generic;
using Benchmark5_ContentManagement.Scripts.Authoring;
using Common.Scripts;
using Unity.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Benchmark5_ContentManagement.Scripts.Systems
{
    [DisableAutoCreation]
    public partial class SceneContentApiSystem : SystemBase
    {
        private bool loggedLoadedScene = false;
        private Scene loadedScene;
        private List<Scene> loadedAdditiveScenes = new List<Scene>();
        protected override void OnUpdate()
        {
            bool needLogLoadedScene = loadedScene.IsValid() && loadedScene.isLoaded;
            if (needLogLoadedScene != loggedLoadedScene)
            {
                LogUtility.ContentManagementLog(loadedScene.name + " is Loaded!!");
                loggedLoadedScene = needLogLoadedScene;
            }
        }

        public void LoadAdditiveScenesAsync()
        {
            var additiveSceneAssets = SystemAPI.GetSingletonBuffer<AdditiveSceneAsset>();
            for (int i = 0; i < additiveSceneAssets.Length; i++)
            {
                var sceneAsset = additiveSceneAssets[i];
                if (!sceneAsset.sceneAssetRef.IsReferenceValid)
                    continue;
               
                if(!sceneAsset.startedLoad)
                {
                    LogUtility.ContentManagementLog($"LoadAdditiveScenesAsync: {sceneAsset.sceneAssetRef.ToString()}");
                    Scene scene = sceneAsset.sceneAssetRef.LoadAsync(new Unity.Loading.ContentSceneParameters()
                    {
                        autoIntegrate = true,
                        loadSceneMode = UnityEngine.SceneManagement.LoadSceneMode.Additive
                    });
                    sceneAsset.startedLoad = true;
                    additiveSceneAssets[i] = sceneAsset;
                    loadedAdditiveScenes.Add(scene);
                }
            }
        }

        public void UnLoadAdditiveScenes()
        {
            var additiveSceneAssets = SystemAPI.GetSingletonBuffer<AdditiveSceneAsset>();
            for (int i = 0; i < additiveSceneAssets.Length; i++)
            {
                var sceneAsset = additiveSceneAssets[i];
                if (!sceneAsset.sceneAssetRef.IsReferenceValid)
                    continue;
                if(sceneAsset.startedLoad)
                {
                    var loadedAdditiveScene = loadedAdditiveScenes[i];
                    sceneAsset.sceneAssetRef.Unload(ref loadedAdditiveScene);
                    sceneAsset.startedLoad = false;
                    additiveSceneAssets[i] = sceneAsset;
                }
            }
            loadedAdditiveScenes.Clear();
        }

        public void SwitchSceneAsync()
        {
            var switchSceneAsset = SystemAPI.GetSingletonRW<SwitchSceneAsset>();
            if (!switchSceneAsset.ValueRW.sceneAssetRef.IsReferenceValid)
                return;
            if (!switchSceneAsset.ValueRW.startedLoad)
            {
                LogUtility.ContentManagementLog($"SwitchSceneAsync: {switchSceneAsset.ValueRO.sceneAssetRef.ToString()}");
                Scene scene = switchSceneAsset.ValueRW.sceneAssetRef.LoadAsync(new Unity.Loading.ContentSceneParameters()
                {
                    autoIntegrate = true,
                    loadSceneMode = UnityEngine.SceneManagement.LoadSceneMode.Single,
                });
                loadedScene = scene;
                switchSceneAsset.ValueRW.startedLoad = true;
            }
        }
    }
}