using Benchmark5_ContentManagement.Scripts.Authoring;
using Unity.Burst;
using Unity.Entities;
using Unity.Entities.Content;
using Unity.Transforms;
using UnityEngine;

namespace Benchmark5_ContentManagement.Scripts.Systems
{
    [WorldSystemFilter(WorldSystemFilterFlags.Default | WorldSystemFilterFlags.Editor)]
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public partial struct RenderFromWeakObjectReferenceSystem : ISystem
    {
        public void OnCreate(ref SystemState state) { }
        public void OnDestroy(ref SystemState state) { }
        public void OnUpdate(ref SystemState state)
        {
            foreach (var (transform, dec) in SystemAPI.Query<RefRW<LocalToWorld>, RefRW<WeakObjectReferenceData>>())
            {
                if (!dec.ValueRW.startedLoad)
                {
                    dec.ValueRW.meshRef.LoadAsync();
                    dec.ValueRW.materialRef.LoadAsync();
                    dec.ValueRW.startedLoad = true;
                }
                if (dec.ValueRW.meshRef.LoadingStatus == ObjectLoadingStatus.Completed &&
                    dec.ValueRW.materialRef.LoadingStatus == ObjectLoadingStatus.Completed)
                {
                    Graphics.DrawMesh(dec.ValueRO.meshRef.Result,
                        transform.ValueRO.Value, dec.ValueRO.materialRef.Result, 0);
                }
            }
        }
    }
}