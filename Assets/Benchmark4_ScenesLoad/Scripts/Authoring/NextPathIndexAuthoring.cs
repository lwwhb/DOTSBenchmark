using Unity.Entities;
using UnityEngine;

namespace Benchmark4_ScenesLoad.Scripts.Authoring
{
    struct NextPathIndex : IComponentData
    {
        public uint nextIndex;
    }
    public class NextPathIndexAuthoring : MonoBehaviour
    {
        [HideInInspector] public uint nextIndex = 0;
        private class NextPathIndexAuthoringBaker : Baker<NextPathIndexAuthoring>
        {
            public override void Bake(NextPathIndexAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                var data = new NextPathIndex
                {
                    nextIndex = authoring.nextIndex
                };
                AddComponent(entity, data);
            }
        }
    }
}