using Benchmark4_ScenesLoad.Scripts.Systems;
using Unity.Entities;
using UnityEngine;

namespace Benchmark4_ScenesLoad.Scripts.Monobehaviours
{
    public class SceneInstanceLoadSample : MonoBehaviour
    {
        private void Start()
        {
            var sceneInstanceLoadSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<SceneInstanceLoadSystem>();
            World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<SimulationSystemGroup>()
                .AddSystemToUpdateList(sceneInstanceLoadSystem);
        }
    }
}