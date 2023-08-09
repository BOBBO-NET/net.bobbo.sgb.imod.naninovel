using UnityEngine;
using System.Collections.Generic;
using Naninovel;

namespace BobboNet.SGB.IMod.Naninovel
{
    [EditInProjectSettings]
    public class SGBIModConfiguration : Configuration
    {
        [Header("Debug")]
        [Tooltip("The name of the script to return to when selecting the 'Return to Naninovel' option in the IMod Overlay. "
                + "If empty, then this will default to the project's starting script.")]
        [ResourcePopup(ScriptsConfiguration.DefaultPathPrefix, ScriptsConfiguration.DefaultPathPrefix, ResourcePopupAttribute.EmptyValue)]
        public string overlayReturnScriptName = "";

        [Header("Naniscript Return Targets")]
        public List<NaniscriptReturnTarget> returnTargets = new List<NaniscriptReturnTarget>();
    }
}