using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using NaturiumMod.Content.Helpers;

namespace NaturiumMod.Content.Items.PreHardmode.Materials;

public class NaturiumBar : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Materials/NaturiumBar";

    public override void SetStaticDefaults()
    {
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
    }

    public override void SetDefaults()
    {
        Item.Size = new(20, 20);
        Item.maxStack = 99;
        Item.consumable = true;
        Item.value = Item.buyPrice(0, 0, 1, 75);

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
        recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ModContent.ItemType<NaturiumOre>(), 5),
            new(ModContent.ItemType<BarkionsBark>(), 15),
            new(ItemID.CrimtaneBar, 5)
        ], TileID.LivingLoom);
        recipe.Register();

        recipe = CreateRecipe(1);
        recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ModContent.ItemType<NaturiumOre>(), 5),
            new(ModContent.ItemType<BarkionsBark>(), 15),
            new(ItemID.DemoniteBar, 5)
        ], TileID.LivingLoom);
        recipe.Register();
    }
}
