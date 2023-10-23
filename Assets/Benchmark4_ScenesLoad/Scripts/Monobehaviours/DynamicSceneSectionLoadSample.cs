using Benchmark4_ScenesLoad.Scripts.Systems;
using Unity.Entities;
using UnityEngine;

namespace Benchmark4_ScenesLoad.Scripts.Monobehaviours
{
    public class DynamicSceneSectionLoadSample : MonoBehaviour
    {
        private void Start()
        {
            var dynamicSceneSectionLoad =
                World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<DynamicSceneSectionLoadSystem>();
            World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<SimulationSystemGroup>()
                .AddSystemToUpdateList(dynamicSceneSectionLoad);
            var pathFindingSystem =
                World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<PathFindingSystem>();
            World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<SimulationSystemGroup>()
                .AddSystemToUpdateList(pathFindingSystem);
        }
    }
}