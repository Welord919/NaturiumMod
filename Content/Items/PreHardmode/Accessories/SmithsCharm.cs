using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.Cards.LOB;
using NaturiumMod.Content.Items.PreHardmode.Accessories.CharmPieces;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace NaturiumMod.Content.Items.PreHardmode.Accessories;

public class SmithsCharm : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Accessories/SmithsCharm";
    public override void SetDefaults()
    {
        Item.width = 28;
        Item.height = 28;
        Item.rare = ItemRarityID.Orange;
        Item.value = Item.buyPrice(gold: 10);
        Item.accessory = true;
    }

    public override void UpdateInventory(Player player)
    {
        player.AddBuff(ModContent.BuffType<Buffs.SmithsBuff>(), 2);
    }

    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        player.AddBuff(ModContent.BuffType<Buffs.SmithsBuff>(), 2);
    }
    public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
    {
        int[] blocked = new[]
        {
        ModContent.ItemType<CharmPieceAngler>(),
        ModContent.ItemType<CharmPieceArtisan>(),
        ModContent.ItemType<CharmPieceGuardian>(),
        ModContent.ItemType<CharmPieceMystic>(),
        ModContent.ItemType<CharmPieceWarrior>()
    };

        // Reject if either the already-equipped item or the incoming item is in the blocked list
        if (blocked.Contains(equippedItem.type) || blocked.Contains(incomingItem.type))
            return false;

        return true;
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe = RecipeHelper.GetNewRecipe(recipe, [
        new(ModContent.ItemType<CharmPieceAngler>(), 1),
        new(ModContent.ItemType<CharmPieceArtisan>(), 1),
        new(ModContent.ItemType<CharmPieceGuardian>(), 1),
        new(ModContent.ItemType<CharmPieceMystic>(), 1),
        new(ModContent.ItemType<CharmPieceWarrior>(), 1)
        ], TileID.TinkerersWorkbench);
        recipe.Register();
    }
}
