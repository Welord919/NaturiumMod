using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.PreHardmode.Consumables;
using NaturiumMod.Content.Items.PreHardmode.Materials;
using NaturiumMod.Content.Items.PreHardmode.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace NaturiumMod.Content.Items.PreHardmode.Accessories.CharmPieces;

public class CharmPieceWarrior : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Accessories/CharmPiece"; public override void SetDefaults()
    {
        Item.Size = new(20, 26);
        Item.rare = ItemRarityID.Orange;
        Item.value = Item.buyPrice(0, 1, 0, 0);
        Item.maxStack = 1;
        Item.accessory = true;
    }

    public override void UpdateInventory(Player player)
    {
        player.AddBuff(BuffID.Rage, 2);
        player.AddBuff(BuffID.Wrath, 2);
        player.AddBuff(BuffID.Swiftness, 2);
        player.AddBuff(BuffID.Hunter, 2);
        player.AddBuff(BuffID.Inferno, 2);
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe = RecipeHelper.GetNewRecipe(recipe, [
        new(ModContent.ItemType<CharmBase>(), 1),
        new(ItemID.RagePotion, 5),
        new(ItemID.WrathPotion, 5),
        new(ItemID.SwiftnessPotion, 3),
        new(ItemID.HunterPotion, 3),
        new(ItemID.InfernoPotion, 3),
        new(ItemID.Bone, 10),
        new(ItemID.Stinger, 5)
        ], TileID.TinkerersWorkbench);
        recipe.Register();
    }

}
