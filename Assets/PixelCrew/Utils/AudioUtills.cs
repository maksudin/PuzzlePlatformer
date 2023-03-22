using UnityEngine;

namespace PixelCrew.Utils
{
    public class AudioUtills
    {
        public const string SfxSourceTag = "SfxAudioSource";

        public static AudioSource FindSfxSource() =>
            GameObject.FindWithTag(SfxSourceTag).GetComponent<AudioSource>();
    }
}