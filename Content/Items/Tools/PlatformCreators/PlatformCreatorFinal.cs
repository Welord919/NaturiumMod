using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Tools.PlatformCreators;

public class PlatformCreatorFinal : ModItem
{
    internal static bool ReplaceModeStatic = false;
    internal static int SelectedCountStatic = 25;
    public override string Texture => "NaturiumMod/Assets/Items/Tools/PlatformCreators/PlatformCreatorFinal";

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
        string modeText = ReplaceModeStatic
            ? "Mode: Replace (overwrites blocks)" : "Mode: Safe (avoids overwriting)";

        TooltipLine modeLine = new(Mod, "PlatformCreatorMode", modeText)
        {
            OverrideColor = ReplaceModeStatic
                ? new Color(255, 150, 50) : new Color(50, 200, 150)
        };
        tooltips.Add(modeLine);

        TooltipLine countLine = new(Mod, "PlatformCreatorCount", $"Selected: {SelectedCountStatic}")
        {
            OverrideColor = new Color(200, 200, 200)
        };
        tooltips.Add(countLine);

        TooltipLine hint = new(Mod, "PlatformCreatorHint", "Right-click to open wheel. Left-click to place.")
        {
            OverrideColor = new Color(180, 180, 180)
        };
        tooltips.Add(hint);
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();

        recipe.AddTile(TileID.Anvils);

        recipe.AddIngredient(ModContent.ItemType<PlatformCreator>(), 1);
        recipe.AddIngredient(ModContent.ItemType<PlatformCreator2>(), 1);
        recipe.AddIngredient(ModContent.ItemType<PlatformCreator3>(), 1);
        recipe.AddIngredient(ModContent.ItemType<PlatformCreator4>(), 1);
        recipe.AddIngredient(ItemID.SoulofLight, 5);
        recipe.AddIngredient(ItemID.SoulofNight, 5);

        recipe.Register();
    }
}