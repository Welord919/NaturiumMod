using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Tiles;

public class NatSlimeStatue : ModItem
{
    public override string Texture => "NaturiumMod/Assets/Tiles/NatSlimeStatue";

    public override void SetDefaults()
    {
        Item.CloneDefaults(ItemID.ArmorStatue);
        Item.createTile = ModContent.TileType<NatSlimeStatueTile>();
        Item.placeStyle = 0;
    }
}
