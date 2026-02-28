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

public class Nibiru : ModSystem
{
    // Simple Jungle check
    public override void PostWorldGen()
    {
        // Determine dungeon side
        bool dungeonOnLeft = Main.dungeonX < Main.maxTilesX / 2;

        // Ocean search bounds
        int oceanStartX = dungeonOnLeft ? 50 : Main.maxTilesX - 350;
        int oceanEndX = dungeonOnLeft ? 350 : Main.maxTilesX - 50;

        // Find the deepest water column in the ocean
        int bestX = oceanStartX;
        int maxWaterDepth = 0;

        for (int x = oceanStartX; x <= oceanEndX; x++)
        {
            int depth = 0;

            // Count water tiles downward
            for (int y = 0; y < Main.maxTilesY; y++)
            {
                Tile t = Framing.GetTileSafely(x, y);
                if (t.LiquidAmount > 0)
                    depth++;
            }

            if (depth > maxWaterDepth)
            {
                maxWaterDepth = depth;
                bestX = x;
            }
        }

        // Now bestX is the center of the ocean on the dungeon side

        // Choose a Y coordinate in space
        // Space starts around y = 100–150 depending on world size
        int spaceY = 120; // safe for all world sizes

        // Offset upward if your structure is tall
        int structureHeight = 46; // change to your structure's height in tiles
        int anchorY = spaceY - structureHeight;

        // Center horizontally
        int structureWidth = 46; // change to your structure's width in tiles
        int anchorX = bestX - structureWidth / 2;

        // Place the structure
        Generator.GenerateStructure(
            "Assets/Structures/Nibiru",
            new Point16(anchorX, anchorY),
            Mod
        );

        Main.NewText($"Nibiru spawned at {anchorX}, {anchorY}");
    }

}
