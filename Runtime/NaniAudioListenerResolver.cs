using UnityEngine;
using Naninovel;

namespace BobboNet.SGB.IMod.Naninovel
{
    public static class NaniAudioListenerResolver
    {
        /// <summary>
        /// Attempt to find the AudioListener in the Naninovel engine object. 
        /// This should resolve if the default audio listener is used.
        /// </summary>
        /// <returns></returns>
        public static AudioListener Find()
        {
            GameObject audioObject = Engine.FindObject("AudioController");
            if (audioObject == null) return null;

            AudioListener audioListener = audioObject.GetComponent<AudioListener>();
            return audioListener;
        }
    }
}