using Benchmark2_AssetsLoad.Scripts.Components;
using Benchmark2_AssetsLoad.Scripts.MonoBehaviours;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

namespace Benchmark2_AssetsLoad.Scripts.Systems
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public partial struct InputSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PhysicsWorldSingleton>();
            state.EntityManager.AddComponentData(state.SystemHandle, new InputEventBridge()
            {
                handler = Object.FindObjectOfType<InputEventHandler>()
            });
        }
        
        public void OnUpdate(ref SystemState state)
        {
            if (Input.GetMouseButtonDown(0))
            {
                UnityEngine.Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                float rayDistance = 100;
                Entity entity = Raycast(ray.origin, ray.direction * rayDistance);
                if (entity.Equals(Entity.Null))
                {
                    var inputEventBridge = state.EntityManager.GetComponentData<InputEventBridge>(state.SystemHandle);
                    if (inputEventBridge.handler != null)
                    {
                        GameObject go = inputEventBridge.handler.RayCast(ray, rayDistance);
                        if (go != null)
                        {
                            go.GetComponent<MeshRenderer>().material.color = Color.red;
                        }
                    }
                }
                else
                {
                    state.EntityManager.AddComponentData(entity, new CustomColor { customColor = new float4(0, 1, 1, 1) });
                }
            }
        }
        private Entity Raycast(float3 frompos, float3 topos)
        {
            PhysicsWorldSingleton physicsWorldSingleton = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
            CollisionWorld collisionWorld = physicsWorldSingleton.CollisionWorld;
 
            RaycastInput raycastInput = new RaycastInput
            {
                Start = frompos,
                End = topos,
                Filter = new CollisionFilter
                {
                    BelongsTo =  ~0u,  
                    CollidesWith = ~0u,
                    GroupIndex = 0
                }
 
            };
 
            Unity.Physics.RaycastHit raycastHit = new Unity.Physics.RaycastHit();
            if (collisionWorld.CastRay(raycastInput, out raycastHit))
            {
                Entity hitEntity = collisionWorld.Bodies[raycastHit.RigidBodyIndex].Entity;
                return hitEntity;
            }
            else
            {
                return Entity.Null;
            }
        }
    }
}