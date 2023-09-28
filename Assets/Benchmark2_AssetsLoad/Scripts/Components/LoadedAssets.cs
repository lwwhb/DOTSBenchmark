using Unity.Entities;
using UnityEngine;

namespace Benchmark2_AssetsLoad.Scripts.Components
{
    public class LoadedGoAssets : IComponentData
    {
        public GameObject gameObject;
        public GameObject gameObjectInstance;
    }
    public struct LoadedEntityAssets : IComponentData
    {
        public Entity entity;
        public Entity entityInstance;
    }
}