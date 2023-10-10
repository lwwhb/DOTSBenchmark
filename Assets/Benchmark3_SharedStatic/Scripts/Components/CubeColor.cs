using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;

namespace Benchmark3_SharedStatic.Scripts.Components
{
    [MaterialProperty("_BaseColor")]
    public struct CubeColor : IComponentData
    {
        public float4 cubeColor;
    }
}