
using DOTSBenchmark0;
using Unity.Entities;
using UnityEngine;

namespace DOTSBenchmark1
{
    public class Initialized : MonoBehaviour
    {
        public void LoadScene(string sceneName)
        {
            if (sceneName.Equals("AddTagComponents"))
            {
                var cubeGenerateSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<CubeGenerateSystem>();
                var addTagComponentsSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<AddTagComponentsSystem>();
                var disableTagComponentsSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<DisableTagComponentsSystem>();
                var enableTagComponentsSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<EnableTagComponentsSystem>();
                World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<InitializationSystemGroup>()
                    .AddSystemToUpdateList(cubeGenerateSystem);
                World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<InitializationSystemGroup>()
                    .AddSystemToUpdateList(addTagComponentsSystem);
                World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<InitializationSystemGroup>()
                    .AddSystemToUpdateList(disableTagComponentsSystem);
                World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<InitializationSystemGroup>()
                    .AddSystemToUpdateList(enableTagComponentsSystem);
                UnityEngine.SceneManagement.SceneManager.LoadScene("AddComponents");
            }
            else if (sceneName.Equals("AddComponents"))
            {
                var cubeGenerateSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<CubeGenerateSystem>();
                var addComponentsSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<AddComponentsSystem>();
                var disableComponetsSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<DisableComponentsSystem>();
                var enableComponetsSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<EnableComponentsSystem>();
                World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<InitializationSystemGroup>()
                    .AddSystemToUpdateList(cubeGenerateSystem);
                World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<InitializationSystemGroup>()
                    .AddSystemToUpdateList(addComponentsSystem);
                World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<InitializationSystemGroup>()
                    .AddSystemToUpdateList(disableComponetsSystem);
                World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<InitializationSystemGroup>()
                    .AddSystemToUpdateList(enableComponetsSystem);
                UnityEngine.SceneManagement.SceneManager.LoadScene("AddComponents");
            }
            
        }
    }
}
