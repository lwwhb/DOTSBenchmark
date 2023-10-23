using Benchmark4_ScenesLoad.Scripts.Authoring;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Entities.Serialization;

namespace Benchmark4_ScenesLoad.Scripts.Systems
{
    [WorldSystemFilter(WorldSystemFilterFlags.BakingSystem)]
    public partial struct CustomMetadataBakingSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            // 清理先前Bake的元数据信息
            var cleanupMetadataQuery = SystemAPI.QueryBuilder().WithAll<CustomMetadata, SectionMetadataSetup>().Build();
            state.EntityManager.RemoveComponent<CustomMetadata>(cleanupMetadataQuery);

            // 烘焙新的元数据信息
            var metadataQuery = SystemAPI.QueryBuilder().WithAll<CustomMetadata, SceneSection>().Build();
            var customMetadataArray = metadataQuery.ToComponentDataArray<CustomMetadata>(Allocator.Temp);
            var metaDataEntities = metadataQuery.ToEntityArray(Allocator.Temp);

            var sectionQuery = SystemAPI.QueryBuilder().WithAll<SectionMetadataSetup>().Build();

            for (int index = 0; index < metaDataEntities.Length; ++index)
            {
                var sceneSection = state.EntityManager.GetSharedComponent<SceneSection>(metaDataEntities[index]);
                var sectionEntity = SerializeUtility.GetSceneSectionEntity(sceneSection.Section, state.EntityManager,
                    ref sectionQuery, true);
                state.EntityManager.AddComponentData(sectionEntity, customMetadataArray[index]);
            }

            customMetadataArray.Dispose();
            metaDataEntities.Dispose();
        }
    }
}