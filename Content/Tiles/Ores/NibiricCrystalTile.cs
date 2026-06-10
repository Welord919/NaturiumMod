using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Localization;
using Terraria.ID;

namespace NaturiumMod.Content.Tiles;
public class NibiricCrystalTile : ModTile
{
    public override string Texture => "NaturiumMod/Assets/Tiles/NibiricCrystalTile";

    public override void SetStaticDefaults()
    {
        TileID.Sets.Ore[Type] = true;

        Main.tileSolid[Type] = true;
        Main.tileMergeDirt[Type] = true;
        Main.tileBlockLight[Type] = true;
        Main.tileShine[Type] = 900;
        Main.tileShine2[Type] = true;
        Main.tileSpelunker[Type] = true;
        Main.tileOreFinderPriority[Type] = 400;

        AddMapEntry(new Color(220, 240, 255), Language.GetText("Nibiric Crystal"));

        DustType = DustID.Marble;

        HitSound = SoundID.Tink;

        MineResist = 1.5f;
        MinPick = 105;
    }
}
