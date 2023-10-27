using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Entities.Content;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Benchmark5_ContentManagement.Scripts.Authoring
{
    [InternalBufferCapacity(8)]
    public struct AdditiveSceneAsset : IBufferElementData
    {
        public WeakObjectSceneReference sceneAssetRef;
        public Scene scene;
    }
    public class AdditiveSceneAssetsAuthoring : MonoBehaviour
    {
        public List<WeakObjectSceneReference> loadScenesRef;
        private class SubsceneAssetsBaker : Baker<AdditiveSceneAssetsAuthoring>
        {
            public override void Bake(AdditiveSceneAssetsAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                DynamicBuffer<AdditiveSceneAsset>
                    loadScenesData = AddBuffer<AdditiveSceneAsset>(entity);
                loadScenesData.Length = authoring.loadScenesRef.Count;
                for (int i = 0; i < authoring.loadScenesRef.Count; i++)
                {
                    loadScenesData[i] = new AdditiveSceneAsset
                    {
                        sceneAssetRef = authoring.loadScenesRef[i],
                        scene = default
                    };
                }
            }
        }
    }
}