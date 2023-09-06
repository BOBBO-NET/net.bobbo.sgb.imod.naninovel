using System.Threading.Tasks;
using Naninovel;
using System;

namespace BobboNet.SGB.IMod.Naninovel
{
    public static class FrontendModeManager
    {
        //
        //  Constructor
        //

        static FrontendModeManager()
        {
            IModOverlay.AddButtonAction("Return to Naninovel", async () =>
            {
                // Load the IMod config, and get the set return script name
                var config = Engine.GetConfiguration<SGBIModConfiguration>();
                string returnScriptName = config.overlayReturnScriptName;

                // If there IS no return script setup, default to the engine's starting script
                if (string.IsNullOrWhiteSpace(returnScriptName))
                {
                    returnScriptName = Engine.GetService<IScriptManager>().StartGameScriptName;
                }

                // Load back into the desired return script in naninovel
                await EnterNaninovel(returnScriptName);
            });
        }

        //
        //  Public Methods
        //

        // Thanks to the NaniNovel docs for implementing this nearly verbatim
        // (https://naninovel.com/guide/integration-options.html#switching-modes)
        public static async Task EnterSGBGame(string smileGameName, int saveFile = -1, IMod.LoadSGBMapArgs mapLoadParams = null)
        {
            // 1. Set Naninovel input inactive.
            var inputManager = Engine.GetService<IInputManager>();
            inputManager.ProcessInput = false;

            // 2. Stop script player.
            var scriptPlayer = Engine.GetService<IScriptPlayer>();
            scriptPlayer.Stop();

            // 3. Reset state if necessary
            var stateManager = Engine.GetService<IStateManager>();
            var servicesToNotReset = new Type[] { typeof(IStateManager), typeof(SGBSaveBridgeService) };
            await stateManager.ResetStateAsync(exclude: servicesToNotReset);

            // 4. Set NaniNovel camera inactive.
            var naniCamera = Engine.GetService<ICameraManager>().Camera;
            naniCamera.enabled = false;

            // 5. Set NaniNovel audio listener inactive
            var audioListener = NaniAudioListenerResolver.Find();
            if (audioListener != null) audioListener.enabled = false;

            // 6. Update the audio sync state. This is necessary to keep volume levels sync'd
            var sgbAudioBridge = Engine.GetService<SGBAudioBridgeService>();
            sgbAudioBridge.SyncSGBAudioState();

            // 7. Start SGB!
            await SGBManager.LoadSmileGameAsync(smileGameName, saveFile, mapLoadParams);
        }

        // Thanks to the NaniNovel docs for implementing this nearly verbatim
        // (https://naninovel.com/guide/integration-options.html#switching-modes)
        public static async Task EnterNaninovel(string entryScriptName, string entryLabel = null)
        {
            // 1. Set Naninovel input active.
            var inputManager = Engine.GetService<IInputManager>();
            inputManager.ProcessInput = true;

            // 2. Start the script player.
            var scriptPlayer = Engine.GetService<IScriptPlayer>();
            await scriptPlayer.PreloadAndPlayAsync(entryScriptName, label: entryLabel);

            // 3. Set NaniNovel camera active.
            var naniCamera = Engine.GetService<ICameraManager>().Camera;
            naniCamera.enabled = true;

            // 4. Set NaniNovel audio listener active
            var audioListener = NaniAudioListenerResolver.Find();
            if (audioListener != null) audioListener.enabled = true;

            // 5. Update the audio sync state. This is necessary to keep volume levels sync'd
            var sgbAudioBridge = Engine.GetService<SGBAudioBridgeService>();
            sgbAudioBridge.SyncSGBAudioState();

            // 6. TODO - Unload smile game
            await SGBManager.UnloadSmileGameAsync();
        }
    }
}