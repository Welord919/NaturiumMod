using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;

namespace NaturiumMod.Content.Items.Materials;

public class CameliaPetal : ModItem
{
    public override void SetStaticDefaults()
    {
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 20;
    }

    public override void SetDefaults()
    {
        Item.width = 20;
        Item.height = 20;
        Item.maxStack = 999;
        Item.consumable = true;
        Item.value = Item.buyPrice(copper: 75);
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe(1);
        recipe = RecipeUtils.GetNewRecipe(recipe, (ItemID.Daybloom, 2), TileID.WorkBenches);
        recipe.Register();
    }
}
