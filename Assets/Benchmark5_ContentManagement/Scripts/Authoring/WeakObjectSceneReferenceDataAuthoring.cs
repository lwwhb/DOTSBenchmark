using Unity.Entities;
using Unity.Entities.Content;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Benchmark5_ContentManagement.Scripts.Authoring
{
    public struct WeakObjectSceneReferenceData : IComponentData
    {
        public bool startedLoad;
        public WeakObjectSceneReference sceneRef;
    }
    public class WeakObjectSceneReferenceDataAuthoring : MonoBehaviour
    {
        public WeakObjectSceneReference sceneAsset;
        private class WeakObjectSceneReferenceDataAuthoringBaker : Baker<WeakObjectSceneReferenceDataAuthoring>
        {
            public override void Bake(WeakObjectSceneReferenceDataAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new WeakObjectSceneReferenceData
                {
                    startedLoad = false,
                    sceneRef = authoring.sceneAsset
                });
            }
        }
    }
}