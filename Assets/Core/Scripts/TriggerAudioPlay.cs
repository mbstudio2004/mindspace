using System.Collections.Generic;
using UnityEngine;

namespace Nocci
{
    public class TriggerAudioPlay : MonoBehaviour
    {
        public List<AudioClip> audioClips;
        public bool playOnce = true;
        [TextArea]
        public string subtitle;
        private bool _played;
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                var audioSource = GetComponent<AudioSource>();
                if (audioSource == null)
                {
                    audioSource = gameObject.AddComponent<AudioSource>();
                }

                audioSource.clip = audioClips[Random.Range(0, audioClips.Count)];
                audioSource.Play();
                
                SubtitleShower.Instance.Show(subtitle, audioSource.clip.length + 0.5f);
                
                if (playOnce && !_played)
                {
                    _played = true;
                    Destroy(this);
                }
            }
        }
    }
}
