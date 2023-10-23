using System;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Benchmark4_ScenesLoad.Scripts.Authoring
{
    [InternalBufferCapacity(8)]
    public struct WayPoint : IBufferElementData
    {
        public float3 point;
    }

    public class WayPointsAuthoring : MonoBehaviour
    {
        public List<Vector3> wayPoints;
        private class WayPointsAuthoringBaker : Baker<WayPointsAuthoring>
        {
            public override void Bake(WayPointsAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                DynamicBuffer<WayPoint> waypoints = AddBuffer<WayPoint>(entity);
                waypoints.Length = authoring.wayPoints.Count;
                for (int i = 0; i < authoring.wayPoints.Count; i++)
                {
                    waypoints[i] = new WayPoint { point = new float3(authoring.wayPoints[i]) };
                }
            }
        }
    }
}