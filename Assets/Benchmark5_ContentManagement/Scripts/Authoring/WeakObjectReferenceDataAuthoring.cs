using Unity.Entities;
using Unity.Entities.Content;
using UnityEngine;

namespace Benchmark5_ContentManagement.Scripts.Authoring
{
    public struct WeakObjectReferenceData : IComponentData
    {
        public bool startedLoad;
        public WeakObjectReference<Mesh> meshRef;
        public WeakObjectReference<Material> materialRef;
    }
    public class WeakObjectReferenceDataAuthoring : MonoBehaviour
    {
        public WeakObjectReference<Mesh> mesh;
        public WeakObjectReference<Material> material;
        private class WeakObjectReferenceDataAuthoringBaker : Baker<WeakObjectReferenceDataAuthoring>
        {
            public override void Bake(WeakObjectReferenceDataAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new WeakObjectReferenceData
                {
                    startedLoad = false,
                    meshRef = authoring.mesh,
                    materialRef = authoring.material
                });
            }
        }
    }
}