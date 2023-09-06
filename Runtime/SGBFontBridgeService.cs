using UnityEngine;
using Naninovel;

namespace BobboNet.SGB.IMod.Naninovel
{
    /// <summary>
    /// This service connects custom fonts with SGB IMod.
    /// </summary>
    [InitializeAtRuntime]
    public class SGBFontBridgeService : IEngineService
    {
        private Font originalFont;

        //
        //  Constructors
        //

        public SGBFontBridgeService() { }

        //
        //  Interface Methods
        //

        public UniTask InitializeServiceAsync()
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

        //
        //  Private Methods
        //

        private void ConnectToSGB()
        {
            var configIMod = Engine.GetConfiguration<SGBIModConfiguration>();

            // Cache whatever font SGB is using
            originalFont = SGBFontManager.CurrentFont;

            // If we have a custom font to use, apply it to SGB
            if (configIMod.customTextFont != null)
            {
                SGBFontManager.CurrentFont = configIMod.customTextFont;
            }
        }

        private void DisconnectFromSGB()
        {
            // Re-apply the cache'd font from SGB
            SGBFontManager.CurrentFont = originalFont;
        }
    }
}