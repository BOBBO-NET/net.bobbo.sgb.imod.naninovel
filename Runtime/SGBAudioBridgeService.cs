using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Naninovel;
using BobboNet.SGB.IMod;
using UnityEngine.Audio;

namespace BobboNet.SGB.IMod.Naninovel
{
    /// <summary>
    /// This service connects Naninovel's audio with SGB's audio using IMod.
    /// </summary>
    [InitializeAtRuntime]
    public class SGBAudioBridgeService : IEngineService
    {
        private const string mixerGroupNameSFX = "SFX";
        private const string mixerHandleVolumeSFX = "SFX Volume";

        private const string mixerGroupNameBGM = "BGM";
        private const string mixerHandleVolumeBGM = "BGM Volume";

        private IAudioManager audioManager; // Naninovel's audio manager

        //
        //  Constructors
        //

        public SGBAudioBridgeService(IAudioManager audioManager)
        {
            this.audioManager = audioManager;
        }

        //
        //  Interface Methods
        //

        public UniTask InitializeService()
        {
            // Initialize the service here.
            ConnectToSGB();
            return UniTask.CompletedTask;
        }

        public void ResetService()
        {
            // Reset service state here.
        }

        public void DestroyService()
        {
            // Stop the service and release any used resources here.
            DisconnectFromSGB();
        }

        //
        //  Public Methods
        //

        /// <summary>
        /// Applies the audio mixer's volume levels to SGB's audio settings.
        /// This is necessary to perform when loading in to an SGB instance, because otherwise
        /// it will not know about outside audio changes.
        /// </summary>
        public void SyncSGBAudioState()
        {
            // If we can find the volume handle for the SFX group, apply the current volume
            if (audioManager.AudioMixer.GetFloat(mixerHandleVolumeSFX, out float sfxVolumeDb))
            {
                SGBAudioSettings.SetVolumeSFXDecibels(sfxVolumeDb);
                audioManager.SfxVolume = VolumeUtils.DbToVolumeFloat(sfxVolumeDb);
            }

            // If we can find the volume handle for the BGM group, apply the current volume
            if (audioManager.AudioMixer.GetFloat(mixerHandleVolumeBGM, out float bgmVolumeDb))
            {
                SGBAudioSettings.SetVolumeBGMDecibels(bgmVolumeDb);
                audioManager.BgmVolume = VolumeUtils.DbToVolumeFloat(bgmVolumeDb);
            }
        }

        //
        //  Private Methods
        //

        private void ConnectToSGB()
        {
            // If we can find an SFX mixer group, apply it to SGB
            if (TryGetMixerGroup(audioManager.AudioMixer, mixerGroupNameSFX, out AudioMixerGroup groupSFX))
            {
                SGBAudioSettings.SetMixerGroupSFX(groupSFX, mixerHandleVolumeSFX);
            }

            // If we can find a BGM mixer group, apply it to SGB
            if (TryGetMixerGroup(audioManager.AudioMixer, mixerGroupNameBGM, out AudioMixerGroup groupBGM))
            {
                SGBAudioSettings.SetMixerGroupBGM(groupBGM, mixerHandleVolumeBGM);
            }

            SyncSGBAudioState();
        }

        private void DisconnectFromSGB()
        {
            // If the mixer group assigned to SGB's SFX is Naninovel's SFX mixer group, unassign it.
            if (TryGetMixerGroup(audioManager.AudioMixer, mixerGroupNameSFX, out AudioMixerGroup groupSFX) && groupSFX == SGBAudioSettings.GetMixerGroupSFX())
            {
                SGBAudioSettings.SetMixerGroupSFX(null);
            }

            // If the mixer group assigned to SGB's BGM is Naninovel's BGM mixer group, unassign it.
            if (TryGetMixerGroup(audioManager.AudioMixer, mixerGroupNameBGM, out AudioMixerGroup groupBGM) && groupBGM == SGBAudioSettings.GetMixerGroupBGM())
            {
                SGBAudioSettings.SetMixerGroupBGM(null);
            }
        }

        private bool TryGetMixerGroup(AudioMixer mixer, string groupName, out AudioMixerGroup group)
        {
            AudioMixerGroup[] matchingGroups = mixer.FindMatchingGroups(groupName);

            if (matchingGroups == null || matchingGroups.Length == 0)
            {
                group = null;
                return false;
            }
            else
            {
                group = matchingGroups[0];
                return true;
            }
        }
    }
}