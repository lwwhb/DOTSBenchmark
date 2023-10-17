using System;
using Unity.Entities;
using Unity.Entities.Serialization;
using UnityEngine;

namespace Benchmark4_ScenesLoad.Scripts.Authoring
{
    [Serializable]
    public struct SubscenesReferences : IComponentData
    {
        public EntitySceneReference cubeSceneReference;
        public Entity cubeSceneMetaEntity;
        public EntitySceneReference sphereSceneReference;
        public Entity sphereSceneMetaEntity;
        public EntitySceneReference capsuleSceneReference;
        public Entity capsuleSceneMetaEntity;
        public EntitySceneReference cylinderSceneReference;
        public Entity cylinderSceneMetaEntity;
    }
    public class SubscenesLoaderAuthoring : MonoBehaviour
    {
        [SerializeField] 
        public SubscenesReferences references;
        private class SubscenesLoaderBaker : Baker<SubscenesLoaderAuthoring>
        {
            public override void Bake(SubscenesLoaderAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, authoring.references);
            }
        }
    }
}