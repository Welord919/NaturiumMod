using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Generator = StructureHelper.API.Generator;

namespace NaturiumMod.Content.Generation.Structures;

using global::NaturiumMod.Content.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

public class JungleStructures : ModSystem
{
    // Simple Jungle check
    private bool IsJungle(int x, int y)
    {
        Tile t = Framing.GetTileSafely(x, y);

        return t.TileType == TileID.JungleGrass ||
               t.TileType == TileID.Mud ||
               t.WallType == WallID.JungleUnsafe;
    }

    public override void PostWorldGen()
    {
        // CONFIG CHECK
        if (!ModContent.GetInstance<NaturiumConfig>().Structures)
            return;
        // Try up to 500 times to find a good Jungle location
        for (int attempt = 0; attempt < 500; attempt++)
        {
            int x = WorldGen.genRand.Next(200, Main.maxTilesX - 200);

            // "Halfway through the Jungle" → slightly above cavern layer
            int yMin = (int)Main.rockLayer - 120;
            int yMax = (int)Main.rockLayer - 40;

            int y = WorldGen.genRand.Next(yMin, yMax);

            if (!IsJungle(x, y))
                continue;

            // Place your structure
            Generator.GenerateStructure("Assets/Structures/JungleChest1", new Point16(x, y), Mod);

            return; // Only spawn ONE chest
        }
    }
}
