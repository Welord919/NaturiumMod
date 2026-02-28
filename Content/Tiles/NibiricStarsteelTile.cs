using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ObjectData;
using Terraria.Localization;

namespace NaturiumMod.Content.Tiles;

internal class NibiricStarsteelTile : ModTile
{
    public override string Texture => "NaturiumMod/Assets/Tiles/NibiricStarsteelTile";

    public override void SetStaticDefaults()
    {
        Main.tileSolid[Type] = true;
        Main.tileSolidTop[Type] = true;
        Main.tileShine[Type] = 1100;
        Main.tileFrameImportant[Type] = true;

        TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
        TileObjectData.newTile.StyleHorizontal = true;
        TileObjectData.newTile.LavaDeath = false;
        TileObjectData.addTile(Type);

        AddMapEntry(new Color(75, 170, 30), Language.GetText("Nibiric Starsteel"));
    }
}
