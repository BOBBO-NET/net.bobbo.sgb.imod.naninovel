using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Naninovel;
using BobboNet.SGB.IMod;

namespace BobboNet.SGB.IMod.Naninovel
{
    /// <summary>
    /// This service triggers returning to Naninovel from some runtime action.
    /// </summary>
    [InitializeAtRuntime]
    public class NaniReturnService : IEngineService
    {
        //
        //  Constructors
        //

        public NaniReturnService()
        {

        }

        //
        //  Interface Methods
        //

        public UniTask InitializeServiceAsync()
        {
            // Initialize the service here.
            SceneManager.sceneLoaded += OnSceneLoaded;

            return UniTask.CompletedTask;
        }

        public void ResetService()
        {
            // Reset service state here.
        }

        public void DestroyService()
        {
            // Stop the service and release any used resources here.
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        //
        //  Private Methods
        //

        /// <summary>
        /// When a Unity Scene is loaded, read from the SGBIMod config to find out if we should re-enter Naninovel.
        /// This is meant to be called by the SceneManager.sceneLoaded event.
        /// </summary>
        /// <param name="scene">The scene that was just loaded</param>
        /// <param name="mode">HOW the scene was just loaded</param>
        private async void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // Get the return targets from our config, and check to see if this scene maps to one
            var returnTargets = Engine.GetConfiguration<SGBIModConfiguration>().returnTargets;
            var foundTarget = returnTargets.Find(x => x.sceneName == scene.name);

            // If there's no target for this scene, EXIT EARLY.
            if (foundTarget == null) return;

            // Use the return target to determine how we re-enter Naninovel.
            await FrontendModeManager.EnterNaninovel(foundTarget.returnScript);
        }
    }
}