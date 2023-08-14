using System.Threading.Tasks;
using UnityEngine;
using Naninovel;
using BobboNet.SGB.IMod;

namespace BobboNet.SGB.IMod.Naninovel
{
    /// <summary>
    /// This service connects Naninovel's audio with SGB's audio using IMod.
    /// </summary>
    [InitializeAtRuntime]
    public class SGBSaveBridgeService : IEngineService
    {
        private IStateManager stateManager; // Naninovel's game state manager

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

        }

        private void DisconnectFromSGB()
        {

        }
    }
}