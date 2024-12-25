using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using ModdingGang.Content.Items.Weapons;

namespace ModdingGang.Content.Items.Accessories
{
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
            recipe.AddIngredient(ModContent.ItemType<LeodrakesMedallion>(), 1);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();

            Recipe recipe1 = CreateRecipe();
            recipe1.AddIngredient(ModContent.ItemType<ExteriosMedallion>(), 1);
            recipe1.AddTile(TileID.Anvils);
            recipe1.Register();
        }
    }
}