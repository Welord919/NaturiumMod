using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.PreHardmode.Tools.PlatformCreators;

public static class PlatformCreatorHelpers
{
    public static void UseItem(Player player, int platformPlacementCount, bool inReplaceMode)
    {
        // Determine starting tile coordinates from the mouse world position
        Vector2 mouseWorld = Main.MouseWorld;
        int startX = (int)(mouseWorld.X / 16f);
        int startY = (int)(mouseWorld.Y / 16f);

        // Determine placement direction based on mouse position relative to player center.
        // This ensures looking left (mouse left of player) places to the left and vice versa.
        int dir;
        if (mouseWorld.X < player.Center.X)
        {
            dir = -1;
        }
        else if (mouseWorld.X > player.Center.X)
        {
            dir = 1;
        }
        else
        {
            // If exactly aligned, fall back to player's facing direction, then default to right.
            dir = player.direction;
            if (dir == 0)
            {
                dir = 1;
            }
        }

        int platformTileType = TileID.Platforms;
        bool placedAny = false;

        for (int i = 0; i < platformPlacementCount; i++)
        {
            int x = startX + i * dir;
            int y = startY;

            // Bounds check to avoid out-of-range errors.
            if (x < 10 || x > Main.maxTilesX - 10 || y < 10 || y > Main.maxTilesY - 10)
            {
                continue;
            }

            if (inReplaceMode && Main.tile[x, y].HasTile)
            {
                Terraria.WorldGen.KillTile(x, y, fail: false, effectOnly: false, noItem: true);
            }

            if (Terraria.WorldGen.PlaceTile(x, y, platformTileType, mute: true, forced: false, -1, style: 0))
            {
                placedAny = true;
            }
        }

        // Sync placed tiles to other clients if anything was placed
        if (placedAny && Main.netMode == NetmodeID.MultiplayerClient)
        {
            int radius = platformPlacementCount / 2; // radius used for SendTileSquare; covers a square of (2*radius+1) tiles
            int centerX = startX + dir * radius;
            NetMessage.SendTileSquare(-1, centerX, startY, radius);
        }
    }

    public static void AddTooltip(string name, string text, Color color, Mod mod, List<TooltipLine> tooltips)
    {
        TooltipLine tooltip = new(mod, name, text)
        {
            OverrideColor = color
        };
        tooltips.Add(tooltip);
    }

    public static void CanUseItemMessage(bool inReplaceMode)
    {
        (string newText, byte r, byte g, byte b) = inReplaceMode
            ? ("Replace Mode: Will overwrite through blocks and enemies.", (byte)255, (byte)150, (byte)50)
            : ("Safe Mode: Will avoid overwriting blocks and enemies.", (byte)50, (byte)200, (byte)150);

        Main.NewText(newText, r, g, b);

        SoundEngine.PlaySound(SoundID.MenuTick);
    }
}