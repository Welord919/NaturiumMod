using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace NaturiumMod.Content.Items.PreHardmode.Tools.PlatformCreators;

public class PlatformWheelSystem : ModSystem
{
    internal static PlatformWheelSystem Instance;
    private UserInterface _UserInterface;
    private PlatformWheelState _WheelState;
    private bool _Visible;

    // When true left-click uses are ignored (used while menu is open and until mouse is released).
    internal bool BlockLeftClick;
    public bool IsOpen => _Visible;

    public override void OnModLoad()
    {
        Instance = this;
        _UserInterface = new UserInterface();
        _WheelState = new PlatformWheelState();
        _WheelState.Activate();
        _UserInterface.SetState(null);
        _Visible = false;
        BlockLeftClick = false;
    }

    public override void Unload()
    {
        Instance = null;
        _UserInterface = null;
        _WheelState = null;
    }

    // Center the wheel using UI alignment (HAlign/VAlign).
    public void ToggleOpen()
    {
        _Visible = !_Visible;

        if (_Visible)
        {
            _WheelState.SetInitialState(PlatformCreatorFinal.ReplaceModeStatic, PlatformCreatorFinal.SelectedCountStatic);
            _UserInterface.SetState(_WheelState);
            // Block left clicks until mouse release to avoid immediate placement.
            BlockLeftClick = true;
        }
        else
        {
            _UserInterface.SetState(null);
            // Keep blocking until mouse is released so the click that closed the UI doesn't place.
            BlockLeftClick = true;
        }
    }

    public override void UpdateUI(GameTime gameTime)
    {
        if (BlockLeftClick && !Main.mouseLeft)
        {
            BlockLeftClick = false;
        }

        if (_Visible && _UserInterface?.CurrentState != null)
        {
            _UserInterface.Update(gameTime);
        }
    }

    public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
    {
        int mouseTextIndex = layers.FindIndex(layer => layer.Name == "Vanilla: Mouse Text");
        if (mouseTextIndex != -1)
        {
            layers.Insert(mouseTextIndex, new LegacyGameInterfaceLayer(
                "NaturiumMod: Platform Wheel",
                delegate
                {
                    if (_Visible && _UserInterface?.CurrentState != null)
                    {
                        _UserInterface.Draw(Main.spriteBatch, new GameTime());
                    }
                    return true;
                },
                InterfaceScaleType.UI));
        }
    }

    // Called to place platforms (left-click action).
    public void PlacePlatforms(int count, bool replace)
    {
        // Extra safety: never place while the menu is open.
        if (_Visible)
            return;

        PlatformCreatorFinal.ReplaceModeStatic = replace;
        PlatformCreatorFinal.SelectedCountStatic = count;

        Player player = Main.LocalPlayer;
        Vector2 mouseWorld = Main.MouseWorld;
        int startX = (int)(mouseWorld.X / 16f);
        int startY = (int)(mouseWorld.Y / 16f);

        int dir;
        if (mouseWorld.X < player.Center.X) dir = -1;
        else if (mouseWorld.X > player.Center.X) dir = 1;
        else
        {
            dir = player.direction;
            if (dir == 0) dir = 1;
        }

        int platformTileType = TileID.Platforms;
        bool placedAny = false;

        for (int i = 0; i < count; i++)
        {
            int x = startX + i * dir;
            int y = startY;

            if (x < 10 || x > Main.maxTilesX - 10 || y < 10 || y > Main.maxTilesY - 10)
                continue;

            if (replace)
            {
                if (Main.tile[x, y].HasTile)
                    Terraria.WorldGen.KillTile(x, y, fail: false, effectOnly: false, noItem: true);

                if (Terraria.WorldGen.PlaceTile(x, y, platformTileType, mute: true, forced: true, plr: -1, style: 0))
                    placedAny = true;
            }
            else
            {
                if (Terraria.WorldGen.PlaceTile(x, y, platformTileType, mute: true, forced: false, plr: -1, style: 0))
                    placedAny = true;
            }
        }

        if (placedAny && Main.netMode == NetmodeID.MultiplayerClient)
        {
            int radius = Math.Max(1, count / 2);
            int centerX = startX + dir * radius;
            NetMessage.SendTileSquare(-1, centerX, startY, radius);
        }

        SoundEngine.PlaySound(SoundID.Dig, player.position);

        if (_Visible)
            ToggleOpen();
    }
}