using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Graphics;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Rendering;

namespace DOTSBenchmark0
{
    public class CreateEntitiesByScript : MonoBehaviour
    {
        [Range(10, 100)] public int xHalfCount = 40;
        [Range(10, 100)] public int zHalfCount = 40;
        public Mesh mesh;
        public Material material;
        void Start()
        {
            CreateEntities();
        }

        private void CreateEntities()
        {
            var world = World.DefaultGameObjectInjectionWorld;
            var entityManager = world.EntityManager;

            var filterSettings = RenderFilterSettings.Default;
            filterSettings.ShadowCastingMode = ShadowCastingMode.Off;
            filterSettings.ReceiveShadows = false;

            var renderMeshArray = new RenderMeshArray(new[] { material }, new[] { mesh });
            var renderMeshDescription = new RenderMeshDescription
            {
                FilterSettings = filterSettings,
                LightProbeUsage = LightProbeUsage.Off,
            };

            var prototype = entityManager.CreateEntity();
            RenderMeshUtility.AddComponents(
                prototype,
                entityManager,
                renderMeshDescription,
                renderMeshArray,
                MaterialMeshInfo.FromRenderMeshArrayIndices(0, 0));

            var cubes = CollectionHelper.CreateNativeArray<Entity>(4 * xHalfCount * zHalfCount,
                Allocator.Temp);
            entityManager.Instantiate(prototype, cubes);

            int count = 0;
            foreach (var cube in cubes)
            {
                int x = count % (xHalfCount * 2) - xHalfCount;
                int z = count / (xHalfCount * 2) - zHalfCount;
                var position = new float3(x * 1.1f, 0, z * 1.1f);
                entityManager.AddComponent<LocalTransform>(cube);
                entityManager.SetComponentData<LocalTransform>( cube, new LocalTransform
                {
                    Position = position,
                    Rotation = quaternion.identity,
                    Scale = 1
                });
                count++;
            }

            cubes.Dispose();
            entityManager.DestroyEntity(prototype);
        }
    }
}
