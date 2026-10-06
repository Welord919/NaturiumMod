using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Tiles.Ores;

namespace NaturiumMod.Content.Items.Materials;

public class Apoqliphite : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/Materials/Apoqliphite";

    public override void SetStaticDefaults()
    {
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 10;
    }

    public override void SetDefaults()
    {
        Item.Size = new(20, 20);
        Item.maxStack = 99;
        Item.consumable = true;
        Item.value = Item.buyPrice(silver: 30);
        Item.rare = ItemRarityID.LightRed;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.useTurn = true;
        Item.useAnimation = 15;
        Item.useTime = 10;
        Item.autoReuse = true;

        //Item.createTile = ModContent.TileType<StarsteelTile>();
        //Item.placeStyle = 1;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ModContent.ItemType<Qliphite>(), 1),
        new(ItemID.ChlorophyteBar, 8)
        ], TileID.MythrilAnvil);
        recipe.Register();
    }
}