using Bullastrum.Utility;
using UnityEngine;

namespace Bullastrum
{
    public class AudioPlayer : Singleton<AudioPlayer>
    {
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _audioClip;
        [SerializeField] private float _volume = 1f;
        
        public void Play()
        {
            _audioSource.PlayOneShot(_audioClip, _volume);
        }
    }
}
