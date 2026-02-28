using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.PreHardmode.Consumables;
using NaturiumMod.Content.Items.PreHardmode.Materials;
using NaturiumMod.Content.Items.PreHardmode.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace NaturiumMod.Content.Items.PreHardmode.Accessories.CharmPieces;

public class CharmPieceMystic : ModItem
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
        player.AddBuff(BuffID.MagicPower, 2);
        player.AddBuff(BuffID.ManaRegeneration, 2);
        player.AddBuff(BuffID.NightOwl, 2);
        player.AddBuff(BuffID.Summoning, 2);

    }


    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe = RecipeHelper.GetNewRecipe(recipe, [
        new(ModContent.ItemType<CharmBase>(), 1),
        new(ItemID.MagicPowerPotion, 5),
        new(ItemID.ManaRegenerationPotion, 5),
        new(ItemID.NightOwlPotion, 3),
        new(ItemID.SummoningPotion, 3),
        new(ItemID.FallenStar, 10),
        new(ItemID.Amethyst, 5)
        ], TileID.TinkerersWorkbench);
        recipe.Register();
    }


}
