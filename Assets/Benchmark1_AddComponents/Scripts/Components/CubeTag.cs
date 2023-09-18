using Unity.Entities;

namespace DOTSBenchmark1
{
    public struct CubeTag : IComponentData
    {
        
    }
    
    public struct EnableableCubeTag : IComponentData, IEnableableComponent
    {
        
    }
}