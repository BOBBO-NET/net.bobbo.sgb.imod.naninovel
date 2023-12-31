using UnityEngine;
using Naninovel;

namespace BobboNet.SGB.IMod.Naninovel
{
    /// <summary>
    /// The Naniscript target of some NaniReturnService action.
    /// </summary>
    [System.Serializable]
    public class NaniscriptReturnTarget
    {
        /// <summary>
        /// The name of the scene to listen for. If this scene is loaded, then this target will try to return
        /// to the Naniscript defined in `returnScript`.
        /// </summary>
        public string sceneName = "";

        /// <summary>
        /// The name of the Naniscript to load into once the desired scene is loaded.
        /// </summary>
        [ResourcePopup(ScriptsConfiguration.DefaultPathPrefix, ScriptsConfiguration.DefaultPathPrefix)]
        public string returnScript = "";

        /// <summary>
        /// The name of the label in the Naniscript to load into once the desired scene is loaded. Optional.
        /// </summary>
        public string returnLabel = "";
    }
}