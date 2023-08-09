using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using Naninovel;
using BobboNet.SGB.IMod;

namespace BobboNet.SGB.IMod.Naninovel
{
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

        private async void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // Get the return targets from our config, and check to see if this scene maps to one
            var returnTargets = Engine.GetConfiguration<SGBIModConfiguration>().returnTargets;
            var foundTarget = returnTargets.Find(x => x.sceneName == scene.name);

            // If there's no target for this scene, EXIT EARLY.
            if (foundTarget == null) return;

            await FrontendModeManager.EnterNaninovel(foundTarget.returnScript);
        }
    }
}