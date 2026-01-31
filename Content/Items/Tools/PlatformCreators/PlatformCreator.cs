using CloudinaryDotNet.Actions;
using Terraria;
using Terraria.ID;

namespace NaturiumMod.Content.Items.Tools.PlatformCreators;

public class PlatformCreator : PlatformCreatorBase
{
    public override void SetDefaults()
    {
        BuyPrice = new(0, 0, 100, 0);
        PlatformPlacementCount = 25;
        CraftingBarAmount = 10;

        base.SetDefaults();
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