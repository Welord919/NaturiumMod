using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using NaturiumMod.Content.Helpers;

namespace NaturiumMod.Content.Items.PreHardmode.Materials;

public class RoseIcon : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Materials/RoseIcon";

    public override void SetStaticDefaults()
    {
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 100;
    }

    public override void SetDefaults()
    {
        Item.Size = new(12, 12);
        Item.maxStack = 999;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe(1);
        recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ModContent.ItemType<CameliaPetal>(), 20)
        ], TileID.LivingLoom);
        recipe.Register();
    }
}