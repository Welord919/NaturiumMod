using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.PreHardmode.Materials;
using NaturiumMod.Content.Items.PreHardmode.Tools;
using NaturiumMod.Content.Items.PreHardmode.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.PostHardmode.Accessories;

internal class NibiruMedallion : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Accessories/NibiruMedallion";

    public override void SetDefaults()
    {
        Item.Size = new(20, 20);
        Item.rare = ItemRarityID.Orange;
        Item.accessory = true;
        Item.value = 100000;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        var boost = player.GetModPlayer<WeaponBoostPlayer>();
        boost.activeBoosts["Nibiru"] = true;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ModContent.ItemType<CharmBase>(), 1),
            new(ModContent.ItemType<Starsteel>(), 5),
            new(ItemID.SoulofNight, 10)
        ], TileID.Anvils);
        recipe.Register();

    }
}
