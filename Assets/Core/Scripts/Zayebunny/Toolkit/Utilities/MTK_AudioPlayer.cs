using Sirenix.OdinInspector;
using UnityEngine;

namespace Nocci.Zayebunny.Utilities
{
    [RequireComponent(typeof(AudioSource))]
    public class MTK_AudioPlayer : MonoBehaviour
    {
        public bool playOnStart;
        public bool playOnEnable;

        private void Start()
        {
            if (playOnStart) Play();
        }

        private void OnEnable()
        {
            if (playOnEnable) Play();
        }

        private void OnDisable()
        {
            Stop();
        }

        private void OnDestroy()
        {
            Stop();
        }

        [Button]
        private void Play()
        {
            GetComponent<AudioSource>().Play();
        }

        [Button]
        private void Stop()
        {
            GetComponent<AudioSource>().Stop();
        }
    }
}