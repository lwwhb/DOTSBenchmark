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
        private bool contentIsReady;
#endif
        private SceneContentApiSystem sceneContentAPI;
        private void Start()
        {
            
#if ENABLE_CONTENT_DELIVERY
            ContentDeliveryGlobalState.LogFunc = LogUtility.ContentDeliveryLog;
            //如果remoteUrlRoot为空，则从本地加载
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
            if (sceneContentAPI != null)
            {
#if ENABLE_CONTENT_DELIVERY
                if(contentIsReady)
                    sceneContentAPI.LoadAdditiveScenesAsync();
                else
                {
                    LogUtility.ContentDeliveryLogError("Content is not ready");
                }
#else
                sceneContentAPI.LoadAdditiveScenesAsync();
#endif
                
                
            }
        }
        
        public void OnClickUnLoadAdditiveScenes()
        {
            if (sceneContentAPI != null)
            {
#if ENABLE_CONTENT_DELIVERY
                if(contentIsReady)
                    sceneContentAPI.UnLoadAdditiveScenes();
                else
                {
                    LogUtility.ContentDeliveryLogError("Content is not ready");
                }
#else
                sceneContentAPI.UnLoadAdditiveScenes();
#endif
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