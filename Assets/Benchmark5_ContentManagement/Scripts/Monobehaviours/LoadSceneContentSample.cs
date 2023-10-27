using System;
using Benchmark5_ContentManagement.Scripts.Systems;
using Common.Scripts;
using Unity.Entities;
using Unity.Entities.Content;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Benchmark5_ContentManagement.Scripts.Monobehaviours
{
    public class LoadSceneContentSample : MonoBehaviour
    {
#if ENABLE_CONTENT_DELIVERY
        public string remoteUrlRoot;
        public string initialContentSet;
#endif
        private bool contentIsReady;
        private SceneContentApiSystem sceneContentAPI;
        private void Start()
        {
#if ENABLE_CONTENT_DELIVERY
            ContentDeliveryGlobalState.LogFunc = LogUtility.ContentDeliveryLog;
            //如果remoteUrlRoot为空，则从本地加载
            RuntimeContentSystem.LoadContentCatalog(remoteUrlRoot, Application.persistentDataPath + "/content-cache",
                initialContentSet);
            ContentDeliveryGlobalState.Initialize(remoteUrlRoot, Application.persistentDataPath + "/content-cache",
                initialContentSet, s =>
                {
                    if (s >= ContentDeliveryGlobalState.ContentUpdateState.ContentReady)
                    {
                        contentIsReady = true;
                        LogUtility.ContentDeliveryLog($"CurrentDeliveryGlobalState:  {ContentDeliveryGlobalState.CurrentContentUpdateState}     ContentIsReady: {contentIsReady}");
                    }
                });
#endif
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
            sceneContentAPI = World.DefaultGameObjectInjectionWorld.GetOrCreateSystemManaged<SceneContentApiSystem>();
            World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<InitializationSystemGroup>()
                .AddSystemToUpdateList(sceneContentAPI);
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneUnloaded -= OnSceneUnloaded;
        }
        
        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log(scene.name + " Loaded");
        }

        void OnSceneUnloaded(Scene scene)
        {
            Debug.Log(scene.name + " UnLoaded");
        }

        public void OnClickLoadAdditiveScenes()
        {
            if (sceneContentAPI != null && contentIsReady)
            {
                sceneContentAPI.LoadAdditiveScenesAsync();
            }
            else
            {
                LogUtility.ContentDeliveryLogError("Content is not ready");
            }
        }
        
        public void OnClickUnLoadAdditiveScenes()
        {
            if (sceneContentAPI != null && contentIsReady)
            {
                sceneContentAPI.UnLoadAdditiveScenes();
            }
            else
            {
                LogUtility.ContentDeliveryLogError("Content is not ready");
            }
        }
        
        public void OnClickSwitchScene()
        {
            if (sceneContentAPI != null)
            {
                sceneContentAPI.SwitchSceneAsync();
            }
        }
        
    }
}