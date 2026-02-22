using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace NaturiumMod.Content.Tiles;

public class NatSlimeBannerTile : ModTile
{
    public override string Texture => "NaturiumMod/Assets/Tiles/NatSlimeBannerTile";
    
    public override void SetStaticDefaults()
    {
        Main.tileFrameImportant[Type] = true;
        Main.tileNoAttach[Type] = true;
        Main.tileLavaDeath[Type] = true;

        TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2Top);
        TileObjectData.newTile.Height = 3;
        TileObjectData.newTile.CoordinateHeights = [16, 16, 16];
        TileObjectData.addTile(Type);

        AddMapEntry(new Color(200, 200, 200), CreateMapEntryName());
    }
}
