using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using NaturiumMod.Content.Items.PreHardmode.Materials;
using NaturiumMod.Content.Items.PostHardmode.Weapons;
using NaturiumMod.Content.Helpers;

namespace NaturiumMod.Content.Items.PreHardmode.Accessories;

internal class ExteriosMedallion : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Accessories/ExteriosMedallion";

    public override void SetDefaults()
    {
        Item.Size = new(20, 20);
        Item.rare = ItemRarityID.Green;
        Item.accessory = true;
        Item.value = 100000;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.moveSpeed += 0.5f;
        player.GetDamage(DamageClass.Generic) += 0.1f;

        if (player.HeldItem.type == ModContent.ItemType<ExteriosCannon>())
        {
            player.GetKnockback(DamageClass.Ranged) += 0.7f;
        }
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ModContent.ItemType<LeodrakesMedallion>(), 1),
            new(ModContent.ItemType<NaturiumBar>(), 5)
        ], TileID.Anvils);
        recipe.Register();

        recipe = CreateRecipe();
        recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ModContent.ItemType<BarkionsMedallion>(), 1),
            new(ModContent.ItemType<NaturiumBar>(), 10)
        ], TileID.Anvils);
        recipe.Register();
    }
}
