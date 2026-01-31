using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Tools.PlatformCreators;

public class PlatformCreator : ModItem
{
    private bool _InReplaceMode = false;
    private const int PlatformPlacementCount = 25;

    public override void SetDefaults()
    {
        Item.Size = new(40, 40);
        Item.useTime = 12;
        Item.useAnimation = 12;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.value = Item.buyPrice(0, 0, 100, 0);
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
        PlatformCreatorHelpers.UseItem(player, PlatformPlacementCount, _InReplaceMode);
        return true;
    }

    // Show current mode in the tooltip so players can see it while hovering the item.
    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        PlatformCreatorHelpers.ModifyTooltips(tooltips, Mod, _InReplaceMode);
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient(ItemID.WoodPlatform, PlatformPlacementCount);
        recipe.AddIngredient(ItemID.PlatinumBar, 10);
        recipe.AddTile(TileID.Anvils);
        recipe.Register();
    }
}