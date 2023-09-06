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
            SGBMapLoadManager.OnPreLoad.AddApprovalSource(OnSGBMapPreload);

            return UniTask.CompletedTask;
        }

        public void ResetService()
        {
            // Reset service state here.
        }

        public void DestroyService()
        {
            // Stop the service and release any used resources here.
            SGBMapLoadManager.OnPreLoad.RemoveApprovalSource(OnSGBMapPreload);
        }

        //
        //  Private Methods
        //

        /// <summary>
        /// When an SGB map is loaded, read from the SGBIMod config to find out if we should re-enter Naninovel.
        /// This is meant to be called by the SGBMapLoadManager.OnPreLoad event.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private ApproveEventResult OnSGBMapPreload(SGBMapLoadManager.PreLoadApprovalArgs args)
        {
            // Get the return targets from our config, and check to see if this scene maps to one
            var returnTargets = Engine.GetConfiguration<SGBIModConfiguration>().returnTargets;
            var foundTarget = returnTargets.Find(x => x.sceneName == args.Map.name);

            // If there's no target for this map, approve the load.
            if (foundTarget == null) return ApproveEventResult.Approve;

            // OTHERWISE - we don't want to actually load this map - we want to load OUT of SGB and into Naninovel.
            // Use the return target to determine how we re-enter Naninovel.
            var returnLabel = string.IsNullOrWhiteSpace(foundTarget.returnLabel) ? null : foundTarget.returnLabel;
            _ = FrontendModeManager.EnterNaninovel(foundTarget.returnScript, returnLabel);

            // Cancel the loading of this map.
            return ApproveEventResult.Cancel;
        }
    }
}