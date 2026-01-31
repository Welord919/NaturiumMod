using NaturiumMod.Content.Tiles;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Placeable;

internal class DaltonPainting : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/Placeable/DaltonPainting";

    public override void SetStaticDefaults()
    {
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
    }

    public override void SetDefaults()
    {
        Item.Size = new(48, 80);

        Item.useTime = 15;
        Item.useAnimation = 15;

        Item.useStyle = ItemUseStyleID.Swing;

        Item.autoReuse = true;
        Item.useTurn = true;

        Item.maxStack = 999;
        Item.consumable = true;

        Item.createTile = ModContent.TileType<DaltonPaintingTile>();
        Item.placeStyle = 0;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe = RecipeUtils.GetNewRecipe(recipe, (ItemID.SpookyWood, 69), TileID.WorkBenches);
        recipe.Register();
    }
}
