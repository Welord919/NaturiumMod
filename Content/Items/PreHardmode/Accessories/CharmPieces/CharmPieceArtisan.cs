using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.PreHardmode.Consumables;
using NaturiumMod.Content.Items.PreHardmode.Materials;
using NaturiumMod.Content.Items.PreHardmode.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace NaturiumMod.Content.Items.PreHardmode.Accessories.CharmPieces;

public class CharmPieceArtisan : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Accessories/CharmPiece";
    public override void SetDefaults()
    {
        Item.Size = new(20, 26);
        Item.rare = ItemRarityID.Orange;
        Item.value = Item.buyPrice(0, 1, 0, 0);
        Item.maxStack = 1;
        Item.accessory = true;
    }

    public override void UpdateInventory(Player player)
    {
        player.AddBuff(BuffID.Builder, 2);
        player.AddBuff(BuffID.Mining, 2);
        player.AddBuff(BuffID.Dangersense, 2);
        player.AddBuff(BuffID.Shine, 2);
        player.AddBuff(BuffID.Spelunker, 2);

    }


    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ItemID.BuilderPotion, 5),
        new(ItemID.MiningPotion, 5),
        new(ItemID.TrapsightPotion, 3),
        new(ItemID.ShinePotion, 3),
        new(ItemID.SpelunkerPotion, 3),
        new(ItemID.StoneBlock, 25),
        new(ItemID.IronBar, 5),
        new(ModContent.ItemType<NaturiumBar>(), 10)
        ], TileID.TinkerersWorkbench);
        recipe.Register();
    }

}
