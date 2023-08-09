using UnityEngine;
using Naninovel;

namespace BobboNet.SGB.IMod.Naninovel
{
    [EditInProjectSettings]
    public class SGBIModConfiguration : Configuration
    {
        [Header("Debug")]
        [Tooltip("The name of the script to return to when selecting the 'Return to Naninovel' option in the IMod Overlay. "
                + "If empty, then this will default to the project's starting script.")]
        public string overlayReturnScriptName = "";
    }
}