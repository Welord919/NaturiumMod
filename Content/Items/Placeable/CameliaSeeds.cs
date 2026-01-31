using NaturiumMod.Content.Tiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Placeable;

public class CameliaSeeds : ModItem
{
    public override void SetDefaults()
    {
        Item.Size = new(14, 14);
        Item.maxStack = 999;
        Item.rare = ItemRarityID.White;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.useTime = 10;
        Item.useAnimation = 10;
        Item.consumable = true;
        Item.autoReuse = false;

        // places the CameliaTile (stage 0)
        Item.createTile = ModContent.TileType<CameliaTile>();
    }

    // Only allow planting on valid soil (jungle grass / grass / mud)
    public override bool CanUseItem(Player player)
    {
        if (!base.CanUseItem(player))
        {
            return false;
        }

        int i = Player.tileTargetX;
        int j = Player.tileTargetY;

        // the tile we place occupies (i,j) so we check the tile below (j+1)
        if (j + 1 >= Main.maxTilesY || i < 0 || i >= Main.maxTilesX)
        {
            return false;
        }

        Tile tileBelow = Framing.GetTileSafely(i, j + 1);
        return tileBelow.HasTile && tileBelow.TileType is TileID.JungleGrass or TileID.Grass or TileID.Mud;
    }
}
