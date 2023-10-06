using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;

namespace Benchmark2_AssetsLoad.Scripts.Components
{
    [MaterialProperty("_BaseColor")]
    public struct CustomColor : IComponentData
    {
        public float4 customColor;
    }
}