using Naninovel;
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
            public System.DateTime saveTime;
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

        public UniTask InitializeService()
        {
            // Initialize the service here.
            var configIMod = Engine.GetConfiguration<SGBIModConfiguration>();

            // Connect to Naninovel
            stateManager.AddOnGameSerializeTask(OnNaninovelStateSerialize);
            stateManager.AddOnGameDeserializeTask(OnNaninovelDeserializeState);

            // Connect to SGB
            SGBSaveManager.SaveDataOverrideFunc = OnSGBSave;
            SGBSaveManager.ReadSaveInfoOverrideFunc = OnSGBReadSaveInfo;
            SGBSaveManager.LoadDataOverrideFunc = OnSGBLoad;
            SGBSaveManager.MaxSaveFileCount = configIMod.maxSGBSaveSlots;

            // Apply our config to SGB's pause menu options
            SGBPauseMenuOptions.ItemsButton.IsVisible = configIMod.itemsButton.isVisible;
            SGBPauseMenuOptions.ItemsButton.IsInteractable = configIMod.itemsButton.isInteractable;
            SGBPauseMenuOptions.SkillsButton.IsVisible = configIMod.skillsButton.isVisible;
            SGBPauseMenuOptions.SkillsButton.IsInteractable = configIMod.skillsButton.isInteractable;
            SGBPauseMenuOptions.EquipmentButton.IsVisible = configIMod.equipmentButton.isVisible;
            SGBPauseMenuOptions.EquipmentButton.IsInteractable = configIMod.equipmentButton.isInteractable;
            SGBPauseMenuOptions.StatusButton.IsVisible = configIMod.statusButton.isVisible;
            SGBPauseMenuOptions.StatusButton.IsInteractable = configIMod.statusButton.isInteractable;
            SGBPauseMenuOptions.SaveButton.IsVisible = configIMod.saveButton.isVisible;
            SGBPauseMenuOptions.SaveButton.IsInteractable = configIMod.saveButton.isInteractable;
            SGBPauseMenuOptions.ConfigButton.IsVisible = configIMod.configButton.isVisible;
            SGBPauseMenuOptions.ConfigButton.IsInteractable = configIMod.configButton.isInteractable;
            SGBPauseMenuOptions.CloseButton.IsVisible = configIMod.closeButton.isVisible;
            SGBPauseMenuOptions.CloseButton.IsInteractable = configIMod.closeButton.isInteractable;
            SGBPauseMenuOptions.ExitButton.IsVisible = configIMod.exitButton.isVisible;
            SGBPauseMenuOptions.ExitButton.IsInteractable = configIMod.exitButton.isInteractable;

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
            SGBSaveManager.ReadSaveInfoOverrideFunc = null;
            SGBSaveManager.LoadDataOverrideFunc = null;
            SGBPauseMenuOptions.SaveButton.IsVisible = true;
            SGBPauseMenuOptions.SaveButton.IsInteractable = true;
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
            SGBSaveState newSave = new SGBSaveState
            {
                saveIndex = saveIndex,
                saveTime = System.DateTime.Now
            };

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
        /// A callback that's called when SGB wants to know more about a specific save file
        /// </summary>
        /// <param name="saveIndex">The index of the save file to query.</param>
        /// <returns>A class describing information about the given save slot.</returns>
        private SGBSaveInfo OnSGBReadSaveInfo(int saveIndex)
        {
            // If we don't have the requested save file, EXIT EARLY.
            if (!HasSaveAtIndex(saveIndex)) return SGBSaveInfo.NewEmpty(saveIndex);

            // OTHERWISE, we DO have this save file, so return relevant info
            return SGBSaveInfo.NewReal(saveIndex, currentSaveState.saveTime);
        }

        /// <summary>
        /// A callback that's called when SGB wants to deserialize it's game state, and needs to read it from somewhere.
        /// </summary>
        /// <param name="saveIndex">Which SGB save-data is being loaded</param>
        /// <returns>A stream containing the loaded data</returns>
        private Stream OnSGBLoad(int saveIndex)
        {
            // If we don't have a save at the given index, EXIT EARLY
            if (!HasSaveAtIndex(saveIndex)) return null;

            // OTHERWISE - convert our base64 string into some data
            return new MemoryStream(System.Convert.FromBase64String(currentSaveState.serializedState));
        }

        /// <summary>
        /// Do we have an SGB save file stored with the given index?
        /// </summary>
        /// <param name="saveIndex">The index of the SGB save to look for.</param>
        /// <returns>true if we have that save, false otherwise.</returns>
        private bool HasSaveAtIndex(int saveIndex)
        {
            if (currentSaveState == null) return false;
            if (currentSaveState.saveIndex != saveIndex) return false;
            if (string.IsNullOrEmpty(currentSaveState.serializedState)) return false;

            return true;
        }
    }
}