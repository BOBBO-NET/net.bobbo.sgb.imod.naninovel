using System.Threading.Tasks;
using UnityEngine;
using Naninovel;
using BobboNet.SGB.IMod;

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
                await EnterNaninovel();
            });
        }

        //
        //  Public Methods
        //

        public static async Task EnterSGBGame(string smileGameName)
        {
            await SetNaninovelActive(false);            // Pause Naninovel...
            SGBManager.LoadSmileGame(smileGameName);    // ...and start SGB
        }

        public static async Task EnterNaninovel()
        {
            await SetNaninovelActive(true);

            // TODO - Unload smile game
            // SGBManager.UnloadSmileGame( FILL IN );
        }

        //
        //  Private Methods
        //

        private static async Task SetNaninovelActive(bool isActive)
        {
            // Thanks to the NaniNovel docs for implementing this nearly verbatim
            // (https://naninovel.com/guide/integration-options.html#switching-modes)

            // 1. Set Naninovel input active or inactive.
            var inputManager = Engine.GetService<IInputManager>();
            inputManager.ProcessInput = isActive;

            // 2. Start or Stop script player.
            var scriptPlayer = Engine.GetService<IScriptPlayer>();
            if (isActive)
            {
                scriptPlayer.Play();
            }
            else
            {
                scriptPlayer.Stop();
            }

            // 3. Reset state if necessary
            if(!isActive)
            {
                var stateManager = Engine.GetService<IStateManager>();
                await stateManager.ResetStateAsync();
            }

            // 4. Set NaniNovel camera active or inactive.
            var naniCamera = Engine.GetService<ICameraManager>().Camera;
            naniCamera.enabled = isActive;
        }
    }
}