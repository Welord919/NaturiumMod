using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.PreHardmode.Consumables;
using NaturiumMod.Content.Items.PreHardmode.Materials;
using NaturiumMod.Content.Items.PreHardmode.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;


namespace NaturiumMod.Content.Items.PreHardmode.Materials.CharmPieces;

public class CharmPieceAngler : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Materials/CharmPieces/CharmPiece";
    public override void SetDefaults()
    {
        Item.Size = new(20, 26);
        Item.rare = ItemRarityID.Orange;
        Item.value = Item.buyPrice(0, 1, 0, 0);
        Item.maxStack = 1;
    }

    public override void Load()
    {
        ItemID.Sets.ShimmerTransformToItem[ModContent.ItemType<NaturiumBar>()] =
            ModContent.ItemType<NaturiumOre>();
    }


    public override void UpdateInventory(Player player)
    {
        player.AddBuff(BuffID.Fishing, 2);
        player.AddBuff(BuffID.Crate, 2);
        player.AddBuff(BuffID.Sonar, 2);
        player.AddBuff(BuffID.Gills, 2);
        player.AddBuff(BuffID.WaterWalking, 2);
    }
}
