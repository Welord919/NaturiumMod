using Terraria;
using Terraria.ID;

namespace NaturiumMod.Content.Items.Tools.PlatformCreators;

public class PlatformCreator : PlatformCreatorBase
{
    public PlatformCreator()
    {
        BuyPrice = new(0, 0, 100, 0);
        PlatformPlacementCount = 25;
        CraftingBarAmount = 10;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient(ItemID.WoodPlatform, PlatformPlacementCount);
        recipe.AddIngredient(ItemID.IronBar, CraftingBarAmount);
        recipe.AddTile(TileID.Anvils);
        recipe.Register();
    }
}