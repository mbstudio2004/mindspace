using Sirenix.OdinInspector;
using UnityEngine;

namespace Nocci.Zayebunny.Gewdom
{
    public class GewdomController : MonoBehaviour
    {
        [SerializeField] private string seed;


        [Button]
        public void SetSeed()
        {
            Gewdom.SetSeed(seed);
        }

        [Button]
        public void SetRandomSeed()
        {
            Gewdom.SetRandomSeed();
        }

        [Button]
        public void PrintSeed()
        {
            UnityEngine.Debug.Log(Gewdom.GetSeed);
        }

        [Button]
        public void DebugRandom()
        {
            UnityEngine.Debug.Log(Gewdom.RandomValue());
        }

        [Button]
        public void DebugUltraRandom()
        {
            UnityEngine.Debug.Log(Gewdom.URandomValue());
        }
    }
}