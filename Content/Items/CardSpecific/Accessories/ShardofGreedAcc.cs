using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.Cards.Fusion;
using NaturiumMod.Content.Items.Cards.LOB;
using NaturiumMod.Content.Items.CardSpecific.Fusion.FusionAltar;
using NaturiumMod.Content.Items.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Accessories
{
    public class ShardofGreedAcc : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Accessories/ShardofGreed";

        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 26;
            Item.accessory = true;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.buyPrice(gold: 3);
        }
        public override void UpdateEquip(Player player)
        {
            player.GetModPlayer<CardDropPlayer>().CardDropBoost += 0.07f;

        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.coinLuck += 0.05f;
            player.luck += 0.05f;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe = RecipeHelper.GetNewRecipe(recipe, [
                new(ModContent.ItemType<ShardofGreed>(), 10),
                new(ModContent.ItemType<CharmBase>(), 1),
            ], TileID.TinkerersWorkbench);
            recipe.Register();
        }
    }
}
