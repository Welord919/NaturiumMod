using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using NaturiumMod.Content.Helpers;

namespace NaturiumMod.Content.Items.PreHardmode.Materials;

public class MelliniumPiece : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Materials/MelliniumPiece";

    public override void SetStaticDefaults()
    {
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 5;
    }

    public override void SetDefaults()
    {
        Item.Size = new(12, 12);
        Item.rare = ItemRarityID.Blue;
        Item.maxStack = 999;
        Item.value = Item.buyPrice(0, 0, 15, 50);
    }
    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe(1);
        recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ItemID.DesertFossil, 20),
            new(ItemID.GoldBar, 5)
        ], TileID.LivingLoom);
        recipe.Register();
    }
}
