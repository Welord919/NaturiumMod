using System.Collections.Generic;
using NaturiumMod.Content.Helpers;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.PreHardmode.Tools.PlatformCreators;

public class PlatformCreator2 : ModItem
{
    private bool _InReplaceMode = false;
    private const int PlatformPlacementCount = 50;
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Tools/PlatformCreators/PlatformCreator2";

    public override void SetDefaults()
    {
        Item.Size = new(40, 40);
        Item.useTime = 12;
        Item.useAnimation = 12;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.value = Item.buyPrice(0, 0, 110, 0);
        Item.rare = ItemRarityID.Green;
        Item.UseSound = SoundID.Item1;

        Item.noMelee = true;
        Item.createTile = -1;
        Item.autoReuse = true;
        Item.useTurn = true;
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
        PlatformCreatorHelpers.CanUseItemMessage(_InReplaceMode);

        return false;
    }

    public override bool? UseItem(Player player)
    {
        PlatformCreatorHelpers.UseItem(player, PlatformPlacementCount, _InReplaceMode);
        return true;
    }

    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        PlatformCreatorHelpers.AddTooltip(
            "PlatformCreatorMode", $"Mode: {(_InReplaceMode ? "Replace (overwrites blocks)" : "Safe (avoids overwriting)")}",
            _InReplaceMode ? new(255, 150, 50) : new(50, 200, 150),
            Mod, tooltips
        );
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe = RecipeHelper.GetNewRecipe(recipe, [
            new (ItemID.WoodPlatform, PlatformPlacementCount),
            new (ItemID.CrimtaneBar, 10)
        ], TileID.Anvils);
        recipe.Register();

        recipe = CreateRecipe();
        recipe = RecipeHelper.GetNewRecipe(recipe, [
            new (ItemID.WoodPlatform, PlatformPlacementCount),
            new (ItemID.DemoniteBar, 10)
        ], TileID.Anvils);
        recipe.Register();
    }
}