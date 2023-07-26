using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Naninovel;
using Naninovel.Commands;
using BobboNet.SGB.IMod;
using BobboNet.SGB.IMod.Naninovel;

[CommandAlias("enterSGBProject")]
public class EnterSGBProject : Command
{
    [RequiredParameter, Documentation("The name of the SGB project to enter.")]
    public StringParameter Name;

    public override async UniTask ExecuteAsync(AsyncToken asyncToken = default)
    {
        await FrontendModeManager.EnterSGBGame(Name.Value); // Load in to the SGB game
    }
}