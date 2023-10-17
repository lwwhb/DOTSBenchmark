using System;
using Benchmark4_ScenesLoad.Scripts.Systems;
using Unity.Entities;
using UnityEngine;

namespace Benchmark4_ScenesLoad.Scripts.Monobehaviours
{
    public class SceneLoadSample : MonoBehaviour
    {
        private void Start()
        {
            var scenesLoadSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<ScenesLoadSystem>();
            World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<SimulationSystemGroup>()
                .AddSystemToUpdateList(scenesLoadSystem);
        }
    }
}