using Benchmark5_ContentManagement.Scripts.Authoring;
using Unity.Burst;
using Unity.Entities;
using UnityEngine.SceneManagement;

namespace Benchmark5_ContentManagement.Scripts.Systems
{
    [WorldSystemFilter(WorldSystemFilterFlags.Default | WorldSystemFilterFlags.Editor)]
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public partial struct LoadSceneFromWeakObjectReferenceSystem : ISystem
    {
        public void OnCreate(ref SystemState state) { }
        public void OnDestroy(ref SystemState state) { }
        public void OnUpdate(ref SystemState state)
        {
            foreach (var sceneData in SystemAPI.Query<RefRW<WeakObjectSceneReferenceData>>())
            {
                if (!sceneData.ValueRO.startedLoad)
                {
                    Scene scene = sceneData.ValueRW.sceneRef.LoadAsync(new Unity.Loading.ContentSceneParameters()
                    {
                        autoIntegrate = true,
                        loadSceneMode = UnityEngine.SceneManagement.LoadSceneMode.Additive
                    });
                    sceneData.ValueRW.startedLoad = true;
                }
            }
        }
    }

}