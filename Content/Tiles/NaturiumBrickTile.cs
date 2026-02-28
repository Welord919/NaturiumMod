using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Localization;
using Terraria.ID;

namespace NaturiumMod.Content.Tiles;

internal class NaturiumBrickTile : ModTile
{
    public override string Texture => "NaturiumMod/Assets/Tiles/NaturiumBrickTile";

    public override void SetStaticDefaults()
    {
        Main.tileSolid[Type] = true;
        Main.tileMergeDirt[Type] = false;

        DustType = DustID.Chlorophyte;
        HitSound = SoundID.Tink;
        MineResist = 1.5f;
        MinPick = 45;

        AddMapEntry(new Color(75, 170, 30), Language.GetText("Naturium Bricks"));
    }

}
