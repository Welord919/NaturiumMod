using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace NaturiumMod.Content.Items.Materials 
{
    public class NaturiumBar : ModItem 
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
        }

        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 99;
            Item.consumable = true;
            Item.value = Item.buyPrice(silver: 1, copper: 75);

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.useAnimation = 15;
            Item.useTime = 10;
            Item.autoReuse = true;

            Item.createTile = ModContent.TileType<Tiles.NaturiumBarTile>();
            Item.placeStyle = 1;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe(1);
            recipe.AddIngredient(ModContent.ItemType<NaturiumOre>(), 5);
            recipe.AddIngredient(ModContent.ItemType<BarkionsBark>(), 15);
            recipe.AddIngredient(ItemID.CrimtaneBar, 5);
            recipe.AddTile(TileID.LivingLoom);
            recipe.Register();

            Recipe recipe2 = CreateRecipe(1);
            recipe2.AddIngredient(ModContent.ItemType<NaturiumOre>(), 5);
            recipe2.AddIngredient(ModContent.ItemType<BarkionsBark>(), 15);
            recipe2.AddIngredient(ItemID.DemoniteBar, 5);
            recipe2.AddTile(TileID.LivingLoom);
            recipe2.Register();
        }
    }
}