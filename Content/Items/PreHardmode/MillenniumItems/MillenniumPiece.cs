using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using NaturiumMod.Content.Helpers;

namespace NaturiumMod.Content.Items.PreHardmode.MillenniumItems;

public class MillenniumPiece : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Millennium/MillenniumPiece";

    public override void SetStaticDefaults()
    {
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
    }

    public override void SetDefaults()
    {
        Item.Size = new(12, 12);
        Item.rare = ItemRarityID.Yellow;
        Item.maxStack = 999;
        Item.value = Item.buyPrice(0, 0, 30, 0);
    }
    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe(3);
        recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ItemID.Bone, 1),
            new(ItemID.GoldBar, 1),
            new(ItemID.TissueSample, 1),
            new(ItemID.FallenStar, 1),
            new(ItemID.Amber, 1)
        ], TileID.WorkBenches);
        recipe.Register();
    }
}
