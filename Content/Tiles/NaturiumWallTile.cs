using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Tiles;

public class NaturiumWallTile : ModWall
	{
    public override string Texture => "NaturiumMod/Assets/Tiles/NaturiumWallTile";
    public override void SetStaticDefaults() {
		Main.wallHouse[Type] = true;
		VanillaFallbackOnModDeletion = WallID.DiamondGemspark;
        DustType = DustID.Chlorophyte;
        AddMapEntry(new Color(150, 150, 150));
		}
	}