using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using ModdingGang.Content.Items.Materials;
using ModdingGang.Content.Items.Weapons;

namespace ModdingGang.Content.Items.Accessories
{
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
            recipe.AddIngredient(ModContent.ItemType<LeodrakesMedallion>(), 1);
            recipe.AddIngredient(ModContent.ItemType<NaturiumBar>(), 5);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();

            Recipe recipe1 = CreateRecipe();
            recipe1.AddIngredient(ModContent.ItemType<BarkionsMedallion>(), 1);
            recipe.AddIngredient(ModContent.ItemType<NaturiumBar>(), 10);
            recipe1.AddTile(TileID.Anvils);
            recipe1.Register();
        }
    }
}