using System;
using Unity.Entities;
using Unity.Entities.Content;
using Unity.Entities.Serialization;
using UnityEngine;
using UnityEngine.Serialization;

namespace Benchmark2_AssetsLoad.Scripts.Bakers
{
    [Serializable]
    public struct AssetsReferences : IComponentData
    {
        public EntityPrefabReference entityPrefabReference;
        public WeakObjectReference<GameObject> gameObjectPrefabReference;
    }

    public class AssetsAuthoring : MonoBehaviour
    {
        [SerializeField] 
        public AssetsReferences references;

        class Baker : Baker<AssetsAuthoring>
        {
            public override void Bake(AssetsAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, authoring.references);
            }
        }
    }
}