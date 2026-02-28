using NaturiumMod.Content.Items.PreHardmode.Materials;
using NaturiumMod.Content.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

public class NaturiumWall : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Materials/NaturiumWall";

    public override void SetStaticDefaults()
    {
        Item.ResearchUnlockCount = 400; // optional
    }

    public override void SetDefaults()
    {
        Item.width = 16;
        Item.height = 16;

        Item.maxStack = 9999;
        Item.useTurn = true;
        Item.autoReuse = true;
        Item.useAnimation = 15;
        Item.useTime = 7;
        Item.useStyle = ItemUseStyleID.Swing;

        Item.consumable = true;

        // This is the important part:
        Item.createWall = ModContent.WallType<NaturiumWallTile>();

        Item.value = 0;
    }

    public override void AddRecipes()
    {
        CreateRecipe(4)
            .AddIngredient(ModContent.ItemType<NaturiumBrick>(), 1)
            .AddTile(TileID.WorkBenches)
            .Register();
    }
}
