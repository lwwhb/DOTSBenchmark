using Benchmark3_SharedStatic.Scripts.SystemGroups;
using Benchmark3_SharedStatic.Scripts.Systems;
using Unity.Entities;
using UnityEngine;

namespace Benchmark3_SharedStatic.Scripts.MonoBehaviours
{
    public class Initialize : MonoBehaviour
    {
        public void Launch()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("SharedStatic");
        }
    }
}