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
    [RequiredParameter, Doc("The name of the SGB project to enter.")]
    public StringParameter Name;

    [Doc("The index of the SGB save file to load when starting the game. Set to -1 to load into the main menu. Defaults to 0.")]
    public IntegerParameter SaveIndex;

    [Doc("The name of the SGB map to load into.")]
    public StringParameter MapName;

    [ParameterDefaultValue("0"), Doc("Where to place the player on the X axis once the map has loaded.")]
    public IntegerParameter MapStartPositionX;

    [ParameterDefaultValue("0"), Doc("Where to place the player on the Y axis once the map has loaded.")]
    public IntegerParameter MapStartPositionY;

    [ParameterDefaultValue("-1"), Doc("Which way to orient the player once the map has loaded")]
    public IntegerParameter MapStartDirection;

    [ParameterDefaultValue("-1.0"), Doc("Where to place the player vertically once the map has loaded.")]
    public DecimalParameter MapStartHeight;

    public override async UniTask Execute(AsyncToken asyncToken = default)
    {
        // Determine what save index to use
        int saveIndex = Assigned(SaveIndex) ? SaveIndex.Value : 0;

        // Determine the map loading parameters, if any
        LoadSGBMapArgs mapLoadParams = null;
        if (Assigned(MapName)) mapLoadParams = new LoadSGBMapArgs
        {
            MapName = MapName.Value,
            StartPosition = new Vector2Int(MapStartPositionX.Value, MapStartPositionY.Value),
            StartDirection = MapStartDirection.Value,
            StartHeight = MapStartHeight.Value
        };

        await FrontendModeManager.EnterSGBGame(Name.Value, saveIndex, mapLoadParams);
    }
}