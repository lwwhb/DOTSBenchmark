using Benchmark5_ContentManagement.Scripts.Authoring;
using Common.Scripts;
using Unity.Entities;
using UnityEngine.SceneManagement;

namespace Benchmark5_ContentManagement.Scripts.Systems
{
    [DisableAutoCreation]
    public partial class SceneContentApiSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            
        }

        public void LoadAdditiveScenesAsync()
        {
            var additiveSceneAssets = SystemAPI.GetSingletonBuffer<AdditiveSceneAsset>();
            for (int i = 0; i < additiveSceneAssets.Length; i++)
            {
                var sceneAsset = additiveSceneAssets[i];
                if (!sceneAsset.sceneAssetRef.IsReferenceValid)
                    continue;
                
                if (!sceneAsset.scene.IsValid() || !sceneAsset.scene.isLoaded)
                {
                    LogUtility.ContentDeliveryLog($"LoadAdditiveScenesAsync: {sceneAsset.sceneAssetRef.ToString()}");
                    Scene scene = sceneAsset.sceneAssetRef.LoadAsync(new Unity.Loading.ContentSceneParameters()
                    {
                        loadSceneMode = UnityEngine.SceneManagement.LoadSceneMode.Additive
                    });
                    sceneAsset.scene = scene;
                    additiveSceneAssets[i] = sceneAsset;
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

                if (sceneAsset.scene.IsValid() && sceneAsset.scene.isLoaded)
                {
                    sceneAsset.sceneAssetRef.Unload(ref sceneAsset.scene);
                    sceneAsset.scene = default;
                    additiveSceneAssets[i] = sceneAsset;
                }
            }
        }

        public void SwitchSceneAsync()
        {
            var switchSceneAsset = SystemAPI.GetSingletonRW<SwitchSceneAsset>();
            if (!switchSceneAsset.ValueRW.sceneAssetRef.IsReferenceValid)
                return;

            if (!switchSceneAsset.ValueRW.scene.IsValid() && !switchSceneAsset.ValueRW.scene.isLoaded)
            {
                Scene scene = switchSceneAsset.ValueRW.sceneAssetRef.LoadAsync(new Unity.Loading.ContentSceneParameters()
                {
                    loadSceneMode = UnityEngine.SceneManagement.LoadSceneMode.Single
                });
                switchSceneAsset.ValueRW.scene = scene;
            }
        }
    }
}