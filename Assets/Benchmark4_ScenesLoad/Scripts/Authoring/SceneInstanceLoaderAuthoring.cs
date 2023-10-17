using System;
using Unity.Entities;
using Unity.Entities.Serialization;
using UnityEngine;

namespace Benchmark4_ScenesLoad.Scripts.Authoring
{
    [Serializable]
    public struct SubsceneProtoReferences : IComponentData
    {
        public EntitySceneReference sceneInstanceProtoReference;
        public int instanceCount;
    }

    
    public class SceneInstanceLoaderAuthoring : MonoBehaviour
    {
        [SerializeField] 
        public SubsceneProtoReferences references;
        private class SceneInstanceLoaderAuthoringBaker : Baker<SceneInstanceLoaderAuthoring>
        {
            public override void Bake(SceneInstanceLoaderAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, authoring.references);
            }
        }
    }
}