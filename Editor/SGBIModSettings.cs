using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using BobboNet.SGB.IMod.Naninovel;
using Naninovel;

namespace BobboNet.Editor.SGB.IMod.Naninovel
{
    public class SGBIModSettings : ConfigurationSettings<SGBIModConfiguration>
    {
        private NaniscriptReturnTargetListDrawer returnTargetsListDrawer = new NaniscriptReturnTargetListDrawer();

        //
        //  Override Methods
        //

        protected override Dictionary<string, Action<SerializedProperty>> OverrideConfigurationDrawers()
        {
            var drawers = base.OverrideConfigurationDrawers();
            drawers[nameof(SGBIModConfiguration.returnTargets)] = delegate (SerializedProperty property)
            {
                returnTargetsListDrawer.DrawProperty(property, SerializedObject);
            };
            return drawers;
        }
    }
}
