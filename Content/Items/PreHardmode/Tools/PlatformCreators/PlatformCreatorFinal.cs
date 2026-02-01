using System.Collections.Generic;
using Microsoft.Xna.Framework;
using NaturiumMod.Content.Helpers;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.PreHardmode.Tools.PlatformCreators;

public class PlatformCreatorFinal : ModItem
{
    internal static bool ReplaceModeStatic = false;
    internal static int SelectedCountStatic = 25;
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Tools/PlatformCreators/PlatformCreatorFinal";

    public override void SetDefaults()
    {
        Item.Size = new(40, 40);
        Item.useTime = 14;
        Item.useAnimation = 14;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.value = Item.buyPrice(0, 0, 180, 0);
        Item.rare = ItemRarityID.Green;
        Item.UseSound = SoundID.Item1;

        Item.noMelee = true;
        Item.createTile = TileID.Platforms;
        Item.autoReuse = true;
        Item.useTurn = true;
        Item.tileBoost = 2;
        Item.attackSpeedOnlyAffectsWeaponAnimation = true;
    }

    public override bool AltFunctionUse(Player player) => true;

    // Block item use at the earliest stage if the wheel is open or if the system requests left-click blocking.
    public override bool CanUseItem(Player player)
    {
        PlatformWheelSystem sys = PlatformWheelSystem.Instance;
        return (sys is null || !sys.IsOpen && !sys.BlockLeftClick || player.altFunctionUse == 2) && base.CanUseItem(player);
    }

    // Prevent left clicks while the wheel requests blocking; right click toggles wheel.
    public override bool? UseItem(Player player)
    {
        if (player == null)
        {
            return base.UseItem(player);
        }

        if (PlatformWheelSystem.Instance?.BlockLeftClick == true || (PlatformWheelSystem.Instance?.IsOpen == true && player.altFunctionUse != 2))
        {
            return false;
        }

        if (player.altFunctionUse == 2 && Main.myPlayer == player.whoAmI)
        {
            PlatformWheelSystem.Instance?.ToggleOpen();
            SoundEngine.PlaySound(SoundID.MenuOpen);
        }
        else if (Main.myPlayer == player.whoAmI)
        {
            PlatformWheelSystem.Instance?.PlacePlatforms(SelectedCountStatic, ReplaceModeStatic);
        }

        return true;
    }

    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        PlatformCreatorHelpers.AddTooltip(
            "PlatformCreatorMode", $"Mode: {(ReplaceModeStatic ? "Replace (overwrites blocks)" : "Safe (avoids overwriting)")}",
            ReplaceModeStatic ? new(255, 150, 50) : new(50, 200, 150),
            Mod, tooltips
        );

        PlatformCreatorHelpers.AddTooltip(
            "PlatformCreatorCount", $"Selected: {SelectedCountStatic}",
            new(200, 200, 200),
            Mod, tooltips
        );

        PlatformCreatorHelpers.AddTooltip(
            "PlatformCreatorHint", "Right-click to open wheel. Left-click to place.",
            new(180, 180, 180),
            Mod, tooltips
        );
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ModContent.ItemType<PlatformCreator>(), 1),
            new(ModContent.ItemType<PlatformCreator2>(), 1),
            new(ModContent.ItemType<PlatformCreator3>(), 1),
            new(ModContent.ItemType<PlatformCreator4>(), 1),
            new(ItemID.SoulofLight, 5),
            new(ItemID.SoulofNight, 5),
        ], TileID.Anvils);
        recipe.Register();
    }
}