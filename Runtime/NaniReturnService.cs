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

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log($"Caught scene: {scene.name}");
        }
    }
}