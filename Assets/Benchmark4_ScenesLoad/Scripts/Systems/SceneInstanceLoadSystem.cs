using Benchmark4_ScenesLoad.Scripts.Authoring;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Scenes;
using Unity.Transforms;

namespace Benchmark4_ScenesLoad.Scripts.Systems
{
    [DisableAutoCreation]
    public partial struct SceneInstanceLoadSystem : ISystem, ISystemStartStop
    {
        private Random _random;
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<SubsceneProtoReferences>();
            _random.InitState(123456);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (var transform in
                     SystemAPI.Query<RefRW<LocalTransform>>())
            {
                transform.ValueRW.Position += _random.NextFloat3(-0.1f, 0.1f);
            }
        }

        public void OnStartRunning(ref SystemState state)
        {
            var loadParameters = new SceneSystem.LoadParameters
            {
                Flags = SceneLoadFlags.NewInstance
            };
            var references = SystemAPI.GetSingleton<SubsceneProtoReferences>();
            if (references.sceneInstanceProtoReference.IsReferenceValid)
            {
                for (int i = 0; i < references.instanceCount; i++)
                {
                    SceneSystem.LoadSceneAsync(state.WorldUnmanaged, references.sceneInstanceProtoReference, loadParameters);
                }
            }
        }

        public void OnStopRunning(ref SystemState state)
        {
        }
    }
}