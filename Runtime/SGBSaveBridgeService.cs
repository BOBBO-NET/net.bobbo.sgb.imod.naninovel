using System.Threading.Tasks;
using UnityEngine;
using Naninovel;
using BobboNet.SGB.IMod;
using System.IO;

namespace BobboNet.SGB.IMod.Naninovel
{
    /// <summary>
    /// This service connects Naninovel's audio with SGB's audio using IMod.
    /// </summary>
    [InitializeAtRuntime]
    public class SGBSaveBridgeService : IEngineService
    {
        //
        //  Classes
        //

        [System.Serializable]
        private class SGBSaveState
        {
            public int saveIndex;
            public string serializedState;
        }

        //
        //  Variables
        //

        private IStateManager stateManager; // Naninovel's game state manager
        private SGBSaveState currentSaveState = null; // The last SGB save state

        //
        //  Constructors
        //

        public SGBSaveBridgeService(IStateManager stateManager)
        {
            this.stateManager = stateManager;
        }

        //
        //  Interface Methods
        //

        public UniTask InitializeServiceAsync()
        {
            // Initialize the service here.

            // Connect to Naninovel
            stateManager.AddOnGameSerializeTask(OnNaninovelStateSerialize);
            stateManager.AddOnGameDeserializeTask(OnNaninovelDeserializeState);

            // Connect to SGB
            SGBSaveManager.SaveDataOverrideFunc = OnSGBSave;
            SGBSaveManager.LoadDataOverrideFunc = OnSGBLoad;

            return UniTask.CompletedTask;
        }

        public void ResetService()
        {
            // Reset service state here.
            currentSaveState = null;
        }

        public void DestroyService()
        {
            // Stop the service and release any used resources here.

            // Disconnect from Naninovel
            stateManager.RemoveOnGameSerializeTask(OnNaninovelStateSerialize);
            stateManager.RemoveOnGameDeserializeTask(OnNaninovelDeserializeState);

            // Disconnect from SGB
            SGBSaveManager.SaveDataOverrideFunc = null;
            SGBSaveManager.LoadDataOverrideFunc = null;
        }

        //
        //  Private Methods
        //

        /// <summary>
        /// A callback that's called when Naninovel serializes the current game state (for saving)
        /// </summary>
        /// <param name="stateMap">The object to serialize the current game state into.</param>
        private void OnNaninovelStateSerialize(GameStateMap stateMap)
        {
            if (currentSaveState == null) return;

            stateMap.SetState(currentSaveState);
        }

        /// <summary>
        /// A callback that's called when Naninovel deserializes a game state (when loading)
        /// </summary>
        /// <param name="stateMap"></param>
        /// <returns></returns>
        private UniTask OnNaninovelDeserializeState(GameStateMap stateMap)
        {
            // Try to load the SGB save
            var foundSave = stateMap.GetState<SGBSaveState>();
            if (foundSave == null) return UniTask.CompletedTask;

            // ...and if there is one, cache it
            currentSaveState = foundSave;
            return UniTask.CompletedTask;
        }

        /// <summary>
        /// A callback that's called when SGB serializes it's game state, and wants to write it somewhere.
        /// </summary>
        /// <param name="saveIndex">Which SGB save-data is being saved</param>
        /// <param name="dataToSave">The raw binary data to be saved.</param>
        private void OnSGBSave(int saveIndex, Stream dataToSave)
        {
            SGBSaveState newSave = new SGBSaveState();
            newSave.saveIndex = saveIndex;

            // Rewind the data to the beginning & convert it to a string we can handle
            using (var conversionBuffer = new MemoryStream())
            {
                dataToSave.Seek(0, SeekOrigin.Begin);
                dataToSave.CopyTo(conversionBuffer);
                newSave.serializedState = System.Convert.ToBase64String(conversionBuffer.ToArray());
            }


            // If all is well, update our cached state
            currentSaveState = newSave;
        }

        /// <summary>
        /// A callback that's called when SGB wants to deserialize it's game state, and needs to read it from somewhere.
        /// </summary>
        /// <param name="saveIndex">Which SGB save-data is being loaded</param>
        /// <returns>A stream containing the loaded data</returns>
        private Stream OnSGBLoad(int saveIndex)
        {
            // If we have no cached save states, EXIT
            if (currentSaveState == null) return null;

            // If our save index mismatches, EXIT
            if (currentSaveState.saveIndex != saveIndex) return null;

            // If we don't have serialized data, EXIT
            if (string.IsNullOrEmpty(currentSaveState.serializedState)) return null;

            // OTHERWISE - convert our base64 string into some data
            return new MemoryStream(System.Convert.FromBase64String(currentSaveState.serializedState));
        }
    }
}