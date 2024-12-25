using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModdingGang.Content.Tiles
{
	// The item used to place the statue.
	public class NatSlimeStatue : ModItem
	{
		public override void SetDefaults()
        {
			Item.CloneDefaults(ItemID.ArmorStatue);
			Item.createTile = ModContent.TileType<NatSlimeStatueTile>();
			Item.placeStyle = 0;
		}
    }
}