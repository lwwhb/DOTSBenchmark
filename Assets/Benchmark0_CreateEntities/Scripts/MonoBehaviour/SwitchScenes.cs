using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace DOTSBenchmark0
{
    public class SwitchScenes : MonoBehaviour
    {
        public void LoadScene(string sceneName)
        {
            if (sceneName.Equals("CreateEntitiesByPrefab"))
            {
                var group =World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<CubeGenerateSystem>();
                World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<InitializationSystemGroup>()
                    .AddSystemToUpdateList(group);
            }
            else if (sceneName.Equals("CreateEntitiesByPrefabWithJobs"))
            {
                var group =World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<CubeGenerateWithJobSystem>();
                World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<InitializationSystemGroup>()
                    .AddSystemToUpdateList(group);
            }
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
    }
}
