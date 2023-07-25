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

    public override async UniTask ExecuteAsync(AsyncToken asyncToken = default)
    {
        // Thanks to the NaniNovel docs for implementing this nearly verbatim
        // (https://naninovel.com/guide/integration-options.html#switching-modes)

        // 1. Disable Naninovel input.
        var inputManager = Engine.GetService<IInputManager>();
        inputManager.ProcessInput = false;

        // 2. Stop script player.
        var scriptPlayer = Engine.GetService<IScriptPlayer>();
        scriptPlayer.Stop();

        // 3. Reset state.
        var stateManager = Engine.GetService<IStateManager>();
        await stateManager.ResetStateAsync();

        // 4. Kill the NaniNovel camera.
        var naniCamera = Engine.GetService<ICameraManager>().Camera;
        naniCamera.enabled = false;

        // 5. Load SGB!
        SGBManager.LoadSmileGame(Name.Value);
    }
}