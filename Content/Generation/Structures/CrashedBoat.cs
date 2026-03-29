using Microsoft.Xna.Framework;
using NaturiumMod.Content.Helpers;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Generator = StructureHelper.API.Generator;

namespace NaturiumMod.Content.Generation.Structures
{
    public class CrashedBoat : ModSystem
    {
        public override void PostWorldGen()
        {
            if (!ModContent.GetInstance<NaturiumConfig>().Structures)
                return;

            bool dungeonOnLeft = Main.dungeonX < Main.maxTilesX / 2;
            bool jungleOnLeft = !dungeonOnLeft;

            // Boat spawns on the OPPOSITE side of the jungle
            bool boatOnLeft = !jungleOnLeft;

            int startX = boatOnLeft ? 50 : Main.maxTilesX - 350;
            int endX = boatOnLeft ? 350 : Main.maxTilesX - 50;

            int chosenX = -1;
            int chosenY = -1;

            for (int x = startX; x < endX; x++)
            {
                int y = FindOceanFloor(x);

                if (y != -1)
                {
                    chosenX = x;
                    chosenY = y;
                    break;
                }
            }

            if (chosenX == -1)
            {
                Main.NewText("No valid underwater ocean floor found for Crashed Boat.");
                return;
            }

            // Submerge the boat slightly into the sand
            int placeY = chosenY - 10;

            Generator.GenerateStructure(
                "Assets/Structures/CrashedBoat",
                new Point16(chosenX, placeY),
                Mod
            );

            Main.NewText($"Crashed Boat spawned underwater at {chosenX}, {placeY}");
        }

        private int FindOceanFloor(int x)
        {
            // Must be far enough from beach
            if (!(x < 300 || x > Main.maxTilesX - 300))
                return -1;

            // Scan downward until we find sand UNDERWATER
            for (int y = 50; y < Main.maxTilesY - 200; y++)
            {
                Tile t = Framing.GetTileSafely(x, y);

                // Must be sand
                if (t.HasTile && t.TileType == TileID.Sand)
                {
                    // Must be underwater (water above this tile)
                    Tile above = Framing.GetTileSafely(x, y - 5);

                    if (above.LiquidAmount > 100) // 100 = deep water
                        return y;
                }
            }

            return -1;
        }
    }
}