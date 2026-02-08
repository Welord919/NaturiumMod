using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.PreHardmode.Consumables;
using NaturiumMod.Content.Items.PreHardmode.Materials;
using NaturiumMod.Content.Items.PreHardmode.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace NaturiumMod.Content.Items.PreHardmode.Materials.CharmPieces;

public class CharmPieceGuardian : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Materials/CharmPieces/CharmPiece";
    public override void SetDefaults()
    {
        Item.Size = new(20, 26);
        Item.rare = ItemRarityID.Orange;
        Item.value = Item.buyPrice(0, 1, 0, 0);
        Item.maxStack = 1;
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
            new(ModContent.ItemType<UltiBuildPotion>(), 30)
        ], TileID.Anvils);
        recipe.Register();
    }
}
