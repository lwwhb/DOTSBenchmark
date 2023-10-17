using Benchmark4_ScenesLoad.Scripts.Systems;
using Unity.Entities;
using UnityEngine;

namespace Benchmark4_ScenesLoad.Scripts.Monobehaviours
{
    public class SceneSectionLoadSample : MonoBehaviour
    {
        private void Start()
        {
            var scenesSectionsLoadSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<SceneSectionsLoadSystem>();
            World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<SimulationSystemGroup>()
                .AddSystemToUpdateList(scenesSectionsLoadSystem);
        }
    }
}