using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using NaturiumMod.Content.Items.Materials.PreHardmodeMaterials;
using NaturiumMod.Content.Items.Weapons.EarlyHardmode;

namespace NaturiumMod.Content.Items.Accessories.PreHardmodeAccessories;

internal class ExteriosMedallion : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 20;
        Item.height = 20;
        Item.rare = ItemRarityID.Green;
        Item.accessory = true;
        Item.value = 100000;
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.moveSpeed += 0.5f;
        player.GetDamage(DamageClass.Generic) += 0.1f; // Increase all damage by 10% for all weapons

        if (player.HeldItem.type == ModContent.ItemType<ExteriosCannon>())
        {
            //player.GetAttackSpeed(DamageClass.Melee) += 0.2f; 
            player.GetKnockback(DamageClass.Ranged) += 0.7f;
            //player.manaCost -= 0.2f;
            //player.GetAttackSpeed(DamageClass.SummonMeleeSpeed) += 1f;
        }
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe = RecipeUtils.GetNewRecipe(recipe, [(ModContent.ItemType<LeodrakesMedallion>(), 1), (ModContent.ItemType<NaturiumBar>(), 5)], TileID.Anvils);
        recipe.Register();

        Recipe recipe1 = CreateRecipe();
        recipe1 = RecipeUtils.GetNewRecipe(recipe1, [(ModContent.ItemType<BarkionsMedallion>(), 1), (ModContent.ItemType<NaturiumBar>(), 10)], TileID.Anvils);
        recipe1.Register();
    }
}
