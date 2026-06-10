using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.Cards.Fusion;
using NaturiumMod.Content.Items.Cards.LOB;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Accessories
{
    public class DuelistHat : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Accessories/DuelistHat";

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.accessory = true;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.buyPrice(gold: 2);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.statDefense += 2;
            player.GetModPlayer<CardDropPlayer>().CardDropBoost += 0.05f;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe = RecipeHelper.GetNewRecipe(recipe, [
                new(ModContent.ItemType<DarkEssence>(), 3),
                new(ModContent.ItemType<LightEssence>(), 3),
            new(ItemID.RedHat, 1)
            ], TileID.TinkerersWorkbench);
            recipe.Register();
        }
    }
}
