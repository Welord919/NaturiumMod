using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Creative;
using NaturiumMod.Content.Helpers;

namespace NaturiumMod.Content.Items.PreHardmode.Materials;

public class CharmBase : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Materials/CharmBase";

    public override void SetStaticDefaults()
    {
        CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
    }

    public override void SetDefaults()
    {
        Item.Size = new(12, 12);
        Item.value = Item.buyPrice(0, 0, 0, 69);
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe(1);
        recipe = RecipeHelper.GetNewRecipe(recipe, [
        new(ModContent.ItemType<BarkionsBark>(), 15),
        new(ItemID.IronBar, 10)
        ], TileID.Anvils);
        recipe.Register();
    }
}
