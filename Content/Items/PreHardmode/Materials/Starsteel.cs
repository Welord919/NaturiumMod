using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using NaturiumMod.Content.Helpers;

namespace NaturiumMod.Content.Items.PreHardmode.Materials;

public class Starsteel : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Materials/NibiricStarsteel";

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

        Item.createTile = ModContent.TileType<Tiles.NibiricStarsteelTile>();
        Item.placeStyle = 1;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe(1);
        recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ModContent.ItemType<NibiricCrystal>(), 5),
            new(ItemID.MeteoriteBar, 10),
            new(ItemID.FallenStar, 1)
        ], TileID.LivingLoom);
        recipe.Register();
    }
}