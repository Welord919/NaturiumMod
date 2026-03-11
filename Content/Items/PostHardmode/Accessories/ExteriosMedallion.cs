using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using NaturiumMod.Content.Items.PreHardmode.Materials;
using NaturiumMod.Content.Items.PostHardmode.Weapons;
using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.PreHardmode.Accessories;
using NaturiumMod.Content.Items.PostHardmode.Materials;

namespace NaturiumMod.Content.Items.PostHardmode.Accessories;

internal class ExteriosMedallion : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/PostHardmode/Accessories/ExteriosMedallion";

    public override void SetDefaults()
    {
        Item.Size = new(20, 20);
        Item.rare = ItemRarityID.LightRed;
        Item.accessory = true;
        Item.value = 100000;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        var boost = player.GetModPlayer<WeaponBoostPlayer>();
        boost.activeBoosts["Exterio"] = true;
    }
    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ModContent.ItemType<CharmBase>(), 1),
            new(ModContent.ItemType<InfusedNaturiumBar>(), 15)
        ], TileID.MythrilAnvil);
        recipe.Register();

        
    }
}
