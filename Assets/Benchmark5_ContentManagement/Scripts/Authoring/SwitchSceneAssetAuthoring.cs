using Unity.Entities;
using Unity.Entities.Content;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Benchmark5_ContentManagement.Scripts.Authoring
{
    public struct SwitchSceneAsset : IComponentData
    {
        public WeakObjectSceneReference sceneAssetRef;
        public Scene scene;
    }
    public class SwitchSceneAssetAuthoring : MonoBehaviour
    {
        public WeakObjectSceneReference switchSceneRef;
        private class SwitchSceneAssetAuthoringBaker : Baker<SwitchSceneAssetAuthoring>
        {
            public override void Bake(SwitchSceneAssetAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new SwitchSceneAsset
                {
                    sceneAssetRef = authoring.switchSceneRef,
                    scene = default
                });
            }
        }
    }
}