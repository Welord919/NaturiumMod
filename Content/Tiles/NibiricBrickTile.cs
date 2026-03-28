using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Tiles
{
    public class NibiricBrickTile : ModTile
    {
        public override string Texture => "NaturiumMod/Assets/Tiles/NibiricBrickTile";

        public override void SetStaticDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = false;
            DustType = DustID.Marble;
            HitSound = SoundID.Tink;
            MineResist = 1.5f;
            MinPick = 105;

            AddMapEntry(new Color(220, 240, 255), Language.GetText("Nibiric Bricks"));

        }
    }
}
