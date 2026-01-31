using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using NaturiumMod.Content.Helpers;

namespace NaturiumMod.Content.Items.PreHardmode.Materials;

public class CameliaPetal : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Materials/CameliaPetal";

    public override void SetStaticDefaults()
    {
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 20;
    }

    public override void SetDefaults()
    {
        Item.Size = new(20, 20);
        Item.maxStack = 999;
        Item.consumable = true;
        Item.value = Item.buyPrice(0, 0, 0, 75);
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe(1);
        recipe = RecipeHelper.GetNewRecipe(recipe, (ItemID.Daybloom, 2), TileID.WorkBenches);
        recipe.Register();
    }
}
