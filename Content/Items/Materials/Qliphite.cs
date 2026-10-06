using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Tiles.Ores;

namespace NaturiumMod.Content.Items.Materials;

public class Qliphite : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/Materials/Qliphite";

    public override void SetStaticDefaults()
    {
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 15;
    }

    public override void SetDefaults()
    {
        Item.Size = new(20, 20);
        Item.maxStack = 99;
        Item.consumable = true;
        Item.value = Item.buyPrice(silver: 25);
        Item.rare = ItemRarityID.Orange;
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
        // Cobalt → Mythril → Adamantite path
        Recipe recipe = CreateRecipe();
        recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ModContent.ItemType<Helshadium>(), 1),
        new(ItemID.CobaltBar, 5),
        new(ItemID.MythrilBar, 5),
        new(ItemID.AdamantiteBar, 5)
        ], TileID.MythrilAnvil);
        recipe.Register();

        // Palladium → Orichalcum → Titanium path
        recipe = CreateRecipe();
        recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ModContent.ItemType<Helshadium>(), 1),
        new(ItemID.PalladiumBar, 5),
        new(ItemID.OrichalcumBar, 5),
        new(ItemID.TitaniumBar, 5)
        ], TileID.MythrilAnvil);
        recipe.Register();
    }
}