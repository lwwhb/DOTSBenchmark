using Benchmark2_AssetsLoad.Scripts.MonoBehaviours;
using Unity.Entities;

namespace Benchmark2_AssetsLoad.Scripts.Components
{
    public class UIEventBridge : IComponentData
    {
        public UIEventHandler handler;
    }
}