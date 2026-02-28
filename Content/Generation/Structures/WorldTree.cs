using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Generator = StructureHelper.API.Generator;

namespace NaturiumMod.Content.Generation.Structures;
public class WorldTree : ModSystem
{
    public override void PostWorldGen()
    {
        bool dungeonOnLeft = Main.dungeonX < Main.maxTilesX / 2;
        bool jungleOnLeft = !dungeonOnLeft;

        List<(int start, int end)> forestSegments = new();

        int x = 50;
        while (x < Main.maxTilesX - 50)
        {
            int surfaceY = FindSurfaceY(x);

            if (IsForest(x, surfaceY + 1))
            {
                int start = x;

                // Continue until forest ends
                while (x < Main.maxTilesX - 50 && IsForest(x, FindSurfaceY(x) + 1))
                    x++;

                int end = x - 1;
                forestSegments.Add((start, end));
            }

            x++;
        }

        if (forestSegments.Count == 0)
        {
            Main.NewText("No forest segments found.");
            return;
        }
        int worldThird = Main.maxTilesX / 3;

        (int start, int end) chosen = default;

        // Filter segments to only the Jungle-side outer third
        List<(int start, int end)> filtered = new();

        if (jungleOnLeft)
        {
            // LEFT outer third
            foreach (var seg in forestSegments)
                if (seg.start >= 50 && seg.end <= worldThird)
                    filtered.Add(seg);
        }
        else
        {
            // RIGHT outer third
            foreach (var seg in forestSegments)
                if (seg.start >= Main.maxTilesX - worldThird && seg.end <= Main.maxTilesX - 50)
                    filtered.Add(seg);
        }

        if (filtered.Count == 0)
        {
            Main.NewText("No forest biome found on jungle side.");
            return;
        }

        // Pick the largest forest segment in that region
        chosen = filtered[0];
        int maxSize = chosen.end - chosen.start;

        foreach (var seg in filtered)
        {
            int size = seg.end - seg.start;
            if (size > maxSize)
            {
                maxSize = size;
                chosen = seg;
            }
        }



        int midX = (chosen.start + chosen.end) / 2;
        int y = FindSurfaceY(midX) - 170;

        int surface = (int)Main.worldSurface;

        if (y > surface && y < 20)
            y = 20;


        Generator.GenerateStructure(
            "Assets/Structures/WorldTreeNeedChests",
            new Point16(midX, y),
            Mod
        );

        Main.NewText($"WorldTree spawned at {midX}, {y}");
    }

    private bool IsForest(int x, int y)
    {
        Tile t = Framing.GetTileSafely(x, y);

        if (!t.HasTile) return false;

        // Must be normal grass
        if (t.TileType != TileID.Grass) return false;

        // Must not have special biome walls
        if (t.WallType != WallID.None) return false;

        // Check nearby tiles for biome-defining blocks
        int radius = 40;
        for (int i = -radius; i <= radius; i++)
        {
            for (int j = -radius; j <= radius; j++)
            {
                Tile n = Framing.GetTileSafely(x + i, y + j);

                if (n.TileType == TileID.SnowBlock ||
                    n.TileType == TileID.IceBlock ||
                    n.TileType == TileID.JungleGrass ||
                    n.TileType == TileID.Mud ||
                    n.TileType == TileID.Sand ||
                    n.TileType == TileID.HardenedSand ||
                    n.TileType == TileID.Sandstone ||
                    n.TileType == TileID.CorruptGrass ||
                    n.TileType == TileID.CrimsonGrass ||
                    n.TileType == TileID.HallowedGrass ||
                    n.TileType == TileID.MushroomGrass)
                    return false;
            }
        }

        return true;
    }


    //Checks for the surface by looking for the first solid tile from the top down
    private int FindSurfaceY(int x)
    {
        int y = 0;
        while (y < Main.maxTilesY - 200 && !Framing.GetTileSafely(x, y).HasTile)
            y++;
        return y;
    }





}
