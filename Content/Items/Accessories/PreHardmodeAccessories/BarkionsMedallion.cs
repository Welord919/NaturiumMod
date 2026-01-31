using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using NaturiumMod.Content.Items.Weapons.PreHardmodeWeapons;

namespace NaturiumMod.Content.Items.Accessories.PreHardmodeAccessories;

internal class BarkionsMedallion : ModItem
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
        player.GetDamage(DamageClass.Generic) += 0.05f; // Increase all damage by 5% for all weapons

        if (player.HeldItem.type != ModContent.ItemType<BarkionsSS>() && player.HeldItem.type != ModContent.ItemType<RoseWhip>() && player.HeldItem.type != ModContent.ItemType<CosmoItem>() && player.HeldItem.type != ModContent.ItemType<BarkionsTB>())
        {
            return;
        }

        player.GetArmorPenetration(DamageClass.Generic) += 1f; // Increases Armour Penetration by 1 points
        player.GetKnockback(DamageClass.Melee) += 0.1f; // Increases melee knockback by 10%
        player.GetAttackSpeed(DamageClass.Ranged) += 0.15f; // Increases ranged attack speed by 15%
        player.manaCost -= 0.2f;
        player.GetDamage(DamageClass.Summon) += 0.1f;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe = RecipeUtils.GetNewRecipe(recipe, (ModContent.ItemType<LeodrakesMedallion>(), 1), TileID.Anvils);
        recipe.Register();

        Recipe recipe1 = CreateRecipe();
        recipe1 = RecipeUtils.GetNewRecipe(recipe1, (ModContent.ItemType<ExteriosMedallion>(), 1), TileID.Anvils);
        recipe1.Register();
    }
}
