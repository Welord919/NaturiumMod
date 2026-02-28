using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Generator = StructureHelper.API.Generator;

namespace NaturiumMod.Content.Generation.Structures;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

public class TempleoftheKings : ModSystem
{
    private bool IsDesert(int x, int y)
    {
        Tile t = Framing.GetTileSafely(x, y);

        return t.TileType == TileID.Sand ||
               t.TileType == TileID.Sandstone ||
               t.TileType == TileID.DesertFossil ||
               t.TileType == TileID.HardenedSand;
    }

    public override void PostWorldGen()
    {
        for (int attempt = 0; attempt < 500; attempt++)
        {
            int x = WorldGen.genRand.Next(200, Main.maxTilesX - 200);
            int yMin = (int)Main.rockLayer - 120;
            int yMax = (int)Main.rockLayer - 40;

            int y = WorldGen.genRand.Next(yMin, yMax);

            if (!IsDesert(x, y))
                continue;

            Generator.GenerateStructure("Assets/Structures/TempleoftheKings", new Point16(x, y), Mod);

            return;
        }
    }
}