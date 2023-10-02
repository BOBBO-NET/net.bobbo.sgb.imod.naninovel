using UnityEngine;
using System.Collections.Generic;
using Naninovel;

namespace BobboNet.SGB.IMod.Naninovel
{
    [EditInProjectSettings]
    public class SGBIModConfiguration : Configuration
    {
        //
        //  Classes
        //

        [System.Serializable]
        public class SGBMenuButton
        {
            public bool isVisible = true;
            public bool isInteractable = true;
        }

        //
        //  Fields
        //

        [Header("Debug")]
        [Tooltip("The name of the script to return to when selecting the 'Return to Naninovel' option in the IMod Overlay. "
                + "If empty, then this will default to the project's starting script.")]
        [ResourcePopup(ScriptsConfiguration.DefaultPathPrefix, ScriptsConfiguration.DefaultPathPrefix, ResourcePopupAttribute.EmptyValue)]
        public string overlayReturnScriptName = "";

        [Header("SGB Fonts")]
        [Tooltip("Custom fonts to use when running SGB projects.")]
        public List<SGBFontTarget> fontTargets = new List<SGBFontTarget>();

        [Header("SGB Save Files")]
        [Tooltip("How many save slots SGB will display at max.")]
        public int maxSGBSaveSlots = 40;

        [Header("SGB Pause Menu")]
        public SGBMenuButton itemsButton = new SGBMenuButton();
        public SGBMenuButton skillsButton = new SGBMenuButton();
        public SGBMenuButton equipmentButton = new SGBMenuButton();
        public SGBMenuButton statusButton = new SGBMenuButton();
        public SGBMenuButton saveButton = new SGBMenuButton();
        public SGBMenuButton configButton = new SGBMenuButton();
        public SGBMenuButton closeButton = new SGBMenuButton();
        public SGBMenuButton exitButton = new SGBMenuButton();

        [Header("Naniscript Return Targets")]
        public List<NaniscriptReturnTarget> returnTargets = new List<NaniscriptReturnTarget>();
    }
}