using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Localization;
using Terraria.ID;

namespace NaturiumMod.Content.Tiles
{
	internal class NaturiumOreTile : ModTile
	{
		public override void SetStaticDefaults()
		{
			TileID.Sets.Ore[Type] = true;

			Main.tileSolid[Type] = true;
			Main.tileMergeDirt[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileShine[Type] = 900;
			Main.tileShine2[Type] = true;
			Main.tileSpelunker[Type] = true;
			Main.tileOreFinderPriority[Type] = 350;

            AddMapEntry(new Color(75, 170, 30), Language.GetText("Naturium Ore"));

            DustType = DustID.Stone;

			HitSound = SoundID.Tink;

			MineResist = 1.5f;
			MinPick = 45;
		}
	}
}