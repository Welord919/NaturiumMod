using Microsoft.Xna.Framework;
using NaturiumMod.Content.Items.Placeable;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace NaturiumMod.Content.Tiles;

public class DaltonPaintingTile : ModTile
{
    public override string Texture => "NaturiumMod/Assets/Tiles/DaltonPaintingTile";

    public override void SetStaticDefaults()
    {
        Main.tileFrameImportant[Type] = true;
        Main.tileNoAttach[Type] = false; // Doesn't attach to other tiles
        Main.tileLavaDeath[Type] = true;

        TileID.Sets.DisableSmartCursor[Type] = true; // Disables smart cursor interaction 
        TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
        TileObjectData.newTile.Width = 3;
        TileObjectData.newTile.Height = 5;
        TileObjectData.newTile.Origin = new Point16(0, 0); // Origin of the tile 
        TileObjectData.newTile.AnchorWall = true; // Ensures the tile can attach to walls, not sure if does anything
        TileObjectData.newTile.UsesCustomCanPlace = true; // Use the default place behavior, not sure if does anything
        TileObjectData.newTile.CoordinateHeights = [16, 16, 16, 16, 6]; // 16 Pixel high rows

        TileObjectData.addTile(Type);

        AddMapEntry(new Color(200, 200, 200), CreateMapEntryName());
    }

    public override void KillMultiTile(int i, int j, int frameX, int frameY)
    {
        Tile tile = Main.tile[i, j];
        if (tile.TileFrameX == 0 && tile.TileFrameY == 0)
        {
            int itemType = ModContent.ItemType<DaltonPainting>();
            Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 48, 70, itemType);
        }
    }
}
