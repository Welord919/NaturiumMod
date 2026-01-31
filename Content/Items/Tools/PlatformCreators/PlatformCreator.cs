using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Tools.PlatformCreators;

public class PlatformCreator : ModItem
{
    private bool _InReplaceMode = false;
    private readonly int _PlatformPlacementCount = 25;
    private readonly int _CraftingBarAmount = 10;
    private readonly BuyPrice BuyPrice = new(0, 0, 100, 0);

    public override void SetDefaults()
    {
        Item.width = 40;
        Item.height = 40;
        Item.useTime = 12;
        Item.useAnimation = 12;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.value = Item.buyPrice(BuyPrice.Platinum, BuyPrice.Gold, BuyPrice.Silver, BuyPrice.Copper);
        Item.rare = ItemRarityID.Green;
        Item.UseSound = SoundID.Item1;

        // Prevent melee hit behavior so the item doesn't "lock on" like a pickaxe.
        Item.noMelee = true;

        // Make the item act like a placeable platform so the game's placement snapping/aiming is used.
        Item.createTile = TileID.Platforms;

        // Allow continuous placement and natural turning while using.
        Item.autoReuse = true;
        Item.useTurn = true;

        // Keep some builder-friendly bonuses
        Item.tileBoost = 2;
        Item.attackSpeedOnlyAffectsWeaponAnimation = true;
    }

    // Enable alternate use (right-click).
    public override bool AltFunctionUse(Player player) => true;

    // Handle right-click toggling of modes.
    public override bool CanUseItem(Player player)
    {
        if (player.altFunctionUse != 2)
        {
            return true;
        }

        // Right-click toggles modes without performing placement.
        _InReplaceMode = !_InReplaceMode;

        // Inform the player of the new mode.
        (string newText, byte r, byte g, byte b) = _InReplaceMode
            ? ("Replace Mode: Will overwrite through blocks and enemies.", (byte)255, (byte)150, (byte)50)
            : ("Safe Mode: Will avoid overwriting blocks and enemies.", (byte)50, (byte)200, (byte)150);

        Main.NewText(newText, r, g, b);

        SoundEngine.PlaySound(SoundID.MenuTick);

        return false;
    }

    // Places 25 platform tiles in a horizontal row starting at the mouse position,
    // extending in the direction the player is looking (determined by mouse position relative to player).
    public override bool? UseItem(Player player)
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

        int platformTileType = TileID.Platforms; // generic platforms tile
        bool placedAny = false;

        for (int i = 0; i < _PlatformPlacementCount; i++)
        {
            int x = startX + i * dir;
            int y = startY;

            // Bounds check to avoid out-of-range errors.
            if (x < 10 || x > Main.maxTilesX - 10 || y < 10 || y > Main.maxTilesY - 10)
            {
                continue;
            }
            
            if (_InReplaceMode && Main.tile[x, y].HasTile)
            {
                // In Replace mode, remove any blocking tile first (no item drop).
                Terraria.WorldGen.KillTile(x, y, fail: false, effectOnly: false, noItem: true);
            }
            
            if (Terraria.WorldGen.PlaceTile(x, y, platformTileType, mute: true, forced: true, -1, style: 0))
            {
                placedAny = true;
            }
        }

        // Sync placed tiles to other clients if anything was placed
        if (placedAny && Main.netMode == NetmodeID.MultiplayerClient)
        {
            int radius = _PlatformPlacementCount / 2; // 12 => covers 25 tiles (2*12+1)
            int centerX = startX + dir * radius;
            NetMessage.SendTileSquare(-1, centerX, startY, radius);
        }

        return true;
    }

    // Show current mode in the tooltip so players can see it while hovering the item.
    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        string modeText = _InReplaceMode
            ? "Mode: Replace (overwrites blocks)" : "Mode: Safe (avoids overwriting)";

        TooltipLine line = new(Mod, "PlatformCreatorMode", modeText)
        {
            OverrideColor = _InReplaceMode
                ? new Color(255, 150, 50) : new Color(50, 200, 150)
        };
        tooltips.Add(line);
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient(ItemID.WoodPlatform, _PlatformPlacementCount);
        recipe.AddIngredient(ItemID.PlatinumBar, _CraftingBarAmount);
        recipe.AddTile(TileID.Anvils);
        recipe.Register();
    }
}