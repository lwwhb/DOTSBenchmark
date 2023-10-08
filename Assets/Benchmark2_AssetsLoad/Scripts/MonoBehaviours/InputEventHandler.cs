using Benchmark2_AssetsLoad.Scripts.Systems;
using Unity.Entities;
using UnityEngine;

namespace Benchmark2_AssetsLoad.Scripts.MonoBehaviours
{
    public class InputEventHandler : MonoBehaviour
    {
        public void Start()
        {
            var inputSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<InputSystem>();
            World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<SimulationSystemGroup>()
                .AddSystemToUpdateList(inputSystem);
        }
        public GameObject RayCast(Ray ray, float distance)
        {
            RaycastHit[] hits = new RaycastHit[1];
            if (Physics.RaycastNonAlloc(ray, hits, distance) > 0)
                return hits[0].collider.gameObject;
            else
                return null;
        }
    }
}