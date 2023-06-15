using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Naninovel;
using Naninovel.Commands;
using BobboNet.SGB.IMod;

[CommandAlias("enterSGBProject")]
public class EnterSGBProject : Command
{
    [RequiredParameter, Documentation("The name of the SGB project to enter.")]
    public StringParameter Name;

    public override UniTask ExecuteAsync(AsyncToken asyncToken = default)
    {
        SGBManager.LoadSmileGame(Name.Value);
        return UniTask.CompletedTask;
    }
}