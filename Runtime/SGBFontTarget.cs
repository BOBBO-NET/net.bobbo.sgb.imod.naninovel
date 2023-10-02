using UnityEngine;
using Naninovel;

namespace BobboNet.SGB.IMod.Naninovel
{
    /// <summary>
    /// The Naniscript target of some NaniReturnService action.
    /// </summary>
    [System.Serializable]
    public class SGBFontTarget
    {
        /// <summary>
        /// The name of the SGB project to target when applying this font.
        /// </summary>
        public string projectName = "";

        /// <summary>
        /// The font to apply when the target SGB project is loaded.
        /// </summary>
        public Font font = null;
    }
}