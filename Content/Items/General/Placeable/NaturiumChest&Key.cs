using NaturiumMod.Content.Items.PreHardmode.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using NaturiumMod.Content.Helpers;

namespace NaturiumMod.Content.Items.General.Placeable;
public class NaturiumChestLocked : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Tiles/NaturiumChestTileLocked";
    public override void SetDefaults()
    {
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.NaturiumChestTile>());
        Item.placeStyle = 1;
        Item.width = 26;
        Item.height = 22;
        Item.value = Item.buyPrice(0, 0, 50, 0);
        Item.maxStack = 99;
    }
    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ModContent.ItemType<NaturiumBar>(), 8)
            .AddIngredient(ModContent.ItemType<NaturiumKey>(), 1)
            .AddTile(TileID.WorkBenches)
            .Register();
    }
}
public class NaturiumChest : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Tiles/NaturiumChest";
    public override void SetDefaults()
    {
        Item.DefaultToPlaceableTile(ModContent.TileType<Tiles.NaturiumChestTile>());
        //Item.placeStyle = 1;
        Item.width = 26;
        Item.height = 22;
        Item.value = Item.buyPrice(0, 0, 50, 0);
        Item.maxStack = 99;
    }

    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ModContent.ItemType<NaturiumBar>(), 8)
            .AddTile(TileID.WorkBenches)
            .Register();
    }
}
public class NaturiumKey : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Tiles/NaturiumKey";
    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.GoldenKey);
        Item.rare = ItemRarityID.Green;
        Item.value = Item.buyPrice(0, 1, 0, 0);
        Item.maxStack = 99;
    }
    public override void AddRecipes()
    {
        CreateRecipe()
            .AddIngredient(ModContent.ItemType<NaturiumBar>(), 2)
            .AddIngredient(ItemID.BeeWax, 5)
            .AddTile(TileID.WorkBenches)
            .Register();
    }
}