using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Tools.PlatformCreators;

public class PlatformCreator2 : ModItem
{
    private bool _InReplaceMode = false;
    private readonly int _PlatformPlacementCount = 50;
    private readonly int _CraftingBarAmount = 10;
    private readonly BuyPrice _BuyPrice = new(0, 0, 110, 0);

    public override void SetDefaults()
    {
        Item.width = 40;
        Item.height = 40;
        Item.useTime = 12;
        Item.useAnimation = 12;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.value = Item.buyPrice(_BuyPrice.Platinum, _BuyPrice.Gold, _BuyPrice.Silver, _BuyPrice.Copper);
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
    public override bool AltFunctionUse(Player player)
    {
        return true;
    }

    // Handle right-click toggling of modes.
    public override bool CanUseItem(Player player)
    {
        if (player.altFunctionUse != 2)
        {
            return true;
        }

        // Right-click toggles modes without performing placement.
        _InReplaceMode = !_InReplaceMode;

        (string newText, byte r, byte g, byte b) = _InReplaceMode
            ? ("Replace Mode: Will overwrite through blocks and enemies.", (byte)255, (byte)150, (byte)50)
            : ("Safe Mode: Will avoid overwriting blocks and enemies.", (byte)50, (byte)200, (byte)150);

        Main.NewText(newText, r, g, b);

        SoundEngine.PlaySound(SoundID.MenuTick);

        // Do not perform the normal left-click action when toggling.
        return false;

    }

    public override bool? UseItem(Player player)
    {
        PlatformCreatorHelpers.UseItem(player, _PlatformPlacementCount, _InReplaceMode);
        return true;
    }

    // Show current mode in the tooltip so players can see it while hovering the item.
    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        PlatformCreatorHelpers.ModifyTooltips(tooltips, Mod, _InReplaceMode);
    }

    public override void AddRecipes()
    {
        // Recipe for Crimtane worlds
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient(ItemID.WoodPlatform, _PlatformPlacementCount);
        recipe.AddIngredient(ItemID.CrimtaneBar, _CraftingBarAmount);
        recipe.AddTile(TileID.Anvils);
        recipe.Register();

        // Recipe for Demonite worlds
        recipe = CreateRecipe();
        recipe.AddIngredient(ItemID.WoodPlatform, _PlatformPlacementCount);
        recipe.AddIngredient(ItemID.DemoniteBar, _CraftingBarAmount);
        recipe.AddTile(TileID.Anvils);
        recipe.Register();
    }
}