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
        private Font defaultFont;

        //
        //  Constructors
        //

        public SGBFontBridgeService() { defaultFont = SGBFontManager.CurrentFont; }

        //
        //  Interface Methods
        //

        public UniTask InitializeService()
        {
            // Initialize the service here.
            return UniTask.CompletedTask;
        }

        public void ResetService()
        {
            // Reset service state here.
        }

        public void DestroyService()
        {
            // Stop the service and release any used resources here.
        }

        //
        //  Public Methods
        //

        public Font GetDefaultFont() => defaultFont;
        public Font GetFont() => SGBFontManager.CurrentFont;
        public void SetFont(Font newFont) => SGBFontManager.CurrentFont = newFont;

        /// <summary>
        /// Try to get a font target for a given SGB project.
        /// </summary>
        /// <param name="projectName">The name / subpath of the SGB project to find a font for.</param>
        /// <param name="foundTarget">The found font target, if there was any/</param>
        /// <returns>True if a target was found, false otherwise.</returns>
        public bool TryGetFontTarget(string projectName, out SGBFontTarget foundTarget)
        {
            var config = Engine.GetConfiguration<SGBIModConfiguration>();

            foreach (var target in config.fontTargets)
            {
                if (target.projectName != projectName) continue;

                foundTarget = target;
                return true;
            }

            foundTarget = null;
            return false;
        }
    }
}