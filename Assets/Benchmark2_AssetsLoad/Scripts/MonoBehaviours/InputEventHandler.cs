using UnityEngine;

namespace Benchmark2_AssetsLoad.Scripts.MonoBehaviours
{
    public class InputEventHandler : MonoBehaviour
    {
        public GameObject RayCast(Ray ray, float distance)
        {
            RaycastHit[] hits = new RaycastHit[1];
            if (Physics.RaycastNonAlloc(ray, hits, distance) > 0)
                return hits[0].collider.gameObject;
            else
                return null;
        }
    }
}