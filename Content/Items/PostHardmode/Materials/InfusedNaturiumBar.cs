using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.PreHardmode.Materials;

namespace NaturiumMod.Content.Items.PostHardmode.Materials;

public class InfusedNaturiumBar : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/PostHardmode/Materials/InfusedNaturiumBar";

    public override void SetStaticDefaults()
    {
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 25;
    }

    public override void SetDefaults()
    {
        Item.Size = new(20, 20);
        Item.maxStack = 99;
        Item.consumable = true;
        Item.value = Item.buyPrice(0, 0, 75, 0);

        Item.useStyle = ItemUseStyleID.Swing;
        Item.useTurn = true;
        Item.useAnimation = 15;
        Item.useTime = 10;
        Item.autoReuse = true;

        Item.createTile = ModContent.TileType<Tiles.InfusedNaturiumBarTile>();
        Item.placeStyle = 1;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe(1);
        recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ModContent.ItemType<NaturiumBar>(), 3),
            new(ModContent.ItemType<NaturesEssence>(), 2),
        ], TileID.MythrilAnvil);
        recipe.Register();
    }
}
