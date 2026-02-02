using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.PreHardmode.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Tiles;

public class NatSlimeStatue : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Tiles/NatSlimeStatue";

    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.ArmorStatue);
        Item.createTile = ModContent.TileType<NatSlimeStatueTile>();
        Item.placeStyle = 0;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe(1);
        recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ModContent.ItemType<NaturiumBar>(), 50),
        ], TileID.Anvils);
        recipe.Register();
    }
}
