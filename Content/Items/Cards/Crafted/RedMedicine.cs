using Microsoft.Xna.Framework;
using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.Cards;
using NaturiumMod.Content.Items.Cards.Fusion;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Cards.LOB.Commons
{
    public class RedMedicine : BaseCardRare
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/RedMedicine";
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.UseSound = SoundID.Item3;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(3);
            recipe = RecipeHelper.GetNewRecipe(recipe, [
                new(ItemID.HealingPotion, 1),
                new(ModContent.ItemType<SpellEssence>(), 5),
            ], ModContent.TileType<FusionAltarTile>());
            recipe.Register();

            Recipe recipe2 = CreateRecipe(3);
            recipe2 = RecipeHelper.GetNewRecipe(recipe2, [
                new(ItemID.LesserHealingPotion, 3),
                new(ModContent.ItemType<SpellEssence>(), 5),
            ], ModContent.TileType<FusionAltarTile>());
            recipe2.Register();

            Recipe recipe3 = CreateRecipe(6);
            recipe3 = RecipeHelper.GetNewRecipe(recipe3, [
                new(ItemID.GreaterHealingPotion, 1),
                new(ModContent.ItemType<SpellEssence>(), 5),
            ], ModContent.TileType<FusionAltarTile>());
            recipe3.Register();
        }
        
        public override bool CanUseItem(Player player)
        {
            if (player.HasBuff(BuffID.PotionSickness)) return false;
            if (player.HasBuff(ModContent.BuffType<SummoningSickness>())) return false;
            return true;
        }

        public override bool? UseItem(Player player)
        {
            player.statLife += 100;
            if (player.statLife > player.statLifeMax2)
                player.statLife = player.statLifeMax2;

            player.HealEffect(75, true);
            player.AddBuff(BuffID.PotionSickness, 45 * 60);
            player.AddBuff(ModContent.BuffType<SummoningSickness>(), 60 * 10);

            return true;
        }
    }
}
