using System;
using Benchmark3_SharedStatic.Scripts.SharedStaticData;
using Benchmark3_SharedStatic.Scripts.SystemGroups;
using Benchmark3_SharedStatic.Scripts.Systems;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Benchmark3_SharedStatic.Scripts.MonoBehaviours
{
    
    public class Launcher : MonoBehaviour
    {
        public uint randomSeed = 0x12345678;
        [Range(10, 100)] public int xHalfCount = 40;
        [Range(10, 100)] public int yHalfCount = 40;
        private const int DyeRedNumber = 10;
        public bool useColorComponentData = false;

        public Action<Entity, float3> notifiyDyeBlueColor;
        public void Start()
        {
            var generateSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<CubesGenerateSystem>();
            World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<InitializationSystemGroup>()
                .AddSystemToUpdateList(generateSystem);
            
            if (useColorComponentData)
            {
                var dyeAndPerformColorSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<DyeAndPerformColorSystem>();
                World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<SimulationSystemGroup>()
                    .AddSystemToUpdateList(dyeAndPerformColorSystem);
            }
            else
            {
                var group = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<SharedStaticDataUpdateSystemGroup>();
                World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<SimulationSystemGroup>()
                    .AddSystemToUpdateList(group);
                
                var dyeRedColorSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<DyeRedColorSystem>();
                World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<SharedStaticDataUpdateSystemGroup>()
                    .AddSystemToUpdateList(dyeRedColorSystem);
                var dyeGreenColorSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<DyeGreenColorSystem>();
                World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<SharedStaticDataUpdateSystemGroup>()
                    .AddSystemToUpdateList(dyeGreenColorSystem);
                var performColorSystem = World.DefaultGameObjectInjectionWorld.GetOrCreateSystem<PerformColorSystem>();
                World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<SharedStaticDataUpdateSystemGroup>()
                    .AddSystemToUpdateList(performColorSystem);
            }
            SharedCubesEntityColorMap.SharedValue.Data = new SharedCubesEntityColorMap(xHalfCount * yHalfCount * 4);
            GlobalSettings.SharedValue.Data.random = new Random(randomSeed);
            GlobalSettings.SharedValue.Data.xHalfSize = xHalfCount;
            GlobalSettings.SharedValue.Data.yHalfSize = yHalfCount;
        }
        public void Update()
        {
            if(!useColorComponentData)
                DyeBlueColor();
        }
        public void OnDestroy()
        {
            SharedCubesEntityColorMap.SharedValue.Data.entityColorMap.Dispose();
        }

        private void DyeBlueColor()
        {
            var entities = SharedCubesEntityColorMap.SharedValue.Data.entityColorMap.GetKeyArray(Allocator.Temp);
            int count = entities.Length;
            if(count < 1)
                return;

            for (int i = 0; i < DyeRedNumber; i++)
            {
                int index = GlobalSettings.SharedValue.Data.random.NextInt(count);
                Entity entity = entities[index];
                float3 color = SharedCubesEntityColorMap.SharedValue.Data.entityColorMap[entity];
                if (color.Equals(new float3(1.0f, 1.0f, 1.0f)))
                    color = new float3(0.0f, 0.0f, 1.0f);
                else
                {
                    color += new float3(0.0f, 0.0f, 1.0f);
                    if (color.z > 1.0f)
                        color.z -= 1.0f;
                }
                SharedCubesEntityColorMap.SharedValue.Data.entityColorMap[entity] = color;
                notifiyDyeBlueColor?.Invoke(entity, color);
            }
        }
    }
}