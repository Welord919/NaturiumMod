using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.PreHardmode.Weapons;
using Terraria;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.PreHardmode.Materials;

public class NaturiumBrick : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Materials/NaturiumBrick";

    public override void SetStaticDefaults()
    {
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 10;
    }

    public override void SetDefaults()
    {
        Item.Size = new(12, 12);
        Item.maxStack = 999;
        Item.consumable = true;
        Item.value = Item.buyPrice(0, 0, 0, 0);

        Item.useStyle = ItemUseStyleID.Swing;
        Item.useAnimation = 15;
        Item.useTime = 10;
        Item.useTurn = true;
        Item.autoReuse = true;

        Item.createTile = ModContent.TileType<Tiles.NaturiumBrickTile>();
    }
    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ItemID.StoneBlock, 4),
            new(ModContent.ItemType<NaturiumOre>(), 1)
        ], TileID.WorkBenches);
        recipe.Register();

        recipe = CreateRecipe();
        recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ModContent.ItemType<NaturiumWall>(), 4)
        ]);
        recipe.Register();
    }
}
