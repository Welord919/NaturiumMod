using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.PreHardmode.Consumables;
using NaturiumMod.Content.Items.PreHardmode.Materials;
using NaturiumMod.Content.Items.PreHardmode.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace NaturiumMod.Content.Items.PreHardmode.Accessories.CharmPieces;

public class CharmPieceGuardian : ModItem
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
        player.AddBuff(BuffID.Ironskin, 2);
        player.AddBuff(BuffID.Endurance, 2);
        player.AddBuff(BuffID.Regeneration, 2);
        player.AddBuff(BuffID.Heartreach, 2);
        player.AddBuff(BuffID.ObsidianSkin, 2);
    }


    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe = RecipeHelper.GetNewRecipe(recipe, [
            new(ItemID.IronskinPotion, 5),
        new(ItemID.EndurancePotion, 5),
        new(ItemID.RegenerationPotion, 5),
        new(ItemID.HeartreachPotion, 3),
        new(ItemID.ObsidianSkinPotion, 3),
        new(ItemID.Obsidian, 15),
        new(ModContent.ItemType<NaturiumBar>(), 10)
        ], TileID.TinkerersWorkbench);
        recipe.Register();
    }

}
