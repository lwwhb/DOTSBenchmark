using Benchmark4_ScenesLoad.Scripts.Systems;
using Unity.Entities;
using UnityEngine;

namespace Benchmark4_ScenesLoad.Scripts.Monobehaviours
{
    public class DynamicSceneLoadSample : MonoBehaviour
    {
        private void Start()
        {
            var dynamicSceneLoad =
                World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<DynamicSceneLoadSystem>();
            World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<SimulationSystemGroup>()
                .AddSystemToUpdateList(dynamicSceneLoad);
            var pathFindingSystem =
                World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<PathFindingSystem>();
            World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<SimulationSystemGroup>()
                .AddSystemToUpdateList(pathFindingSystem);
        }
    }
}