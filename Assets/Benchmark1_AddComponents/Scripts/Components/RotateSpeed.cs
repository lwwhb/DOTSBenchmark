using Unity.Entities;

namespace DOTSBenchmark1
{
    public struct RotateSpeed : IComponentData
    {
        public float speed;
    }
    
    public struct EnableableRotateSpeed : IComponentData, IEnableableComponent
    {
        public float speed;
    }
}