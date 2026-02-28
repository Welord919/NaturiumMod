using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Generator = StructureHelper.API.Generator;

namespace NaturiumMod.Content.Generation.Structures;

public class ForestNatureChestStructures : ModSystem
{
    private readonly List<string> ForestChestSet = new()
    {
        "Assets/Structures/ForestNatureChest1",
        "Assets/Structures/ForestNatureChest1",
        "Assets/Structures/ForestNatureChest1",

        "Assets/Structures/ForestNatureChest2",
        "Assets/Structures/ForestNatureChest2",

        "Assets/Structures/ForestNatureChest3",
        "Assets/Structures/ForestNatureChest3",

        "Assets/Structures/ForestNatureChest4"
    };

    private void Shuffle<T>(IList<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = WorldGen.genRand.Next(i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    private bool IsForest(int x, int y)
    {
        int radius = 40;

        for (int i = -radius; i <= radius; i++)
        {
            for (int j = -radius; j <= radius; j++)
            {
                Tile t = Framing.GetTileSafely(x + i, y + j);
                ushort type = t.TileType;

                // Snow / Ice
                if (type == TileID.SnowBlock || type == TileID.IceBlock ||
                    type == TileID.CorruptIce || type == TileID.FleshIce)
                    return false;

                // Jungle
                if (type == TileID.JungleGrass || type == TileID.Mud)
                    return false;

                // Desert
                if (type == TileID.Sand || type == TileID.HardenedSand || type == TileID.Sandstone ||
                    type == TileID.CorruptHardenedSand || type == TileID.CorruptSandstone ||
                    type == TileID.Crimsand || type == TileID.CrimsonHardenedSand || type == TileID.CrimsonSandstone)
                    return false;

                // Corruption
                if (type == TileID.Ebonstone || type == TileID.EbonstoneBrick ||
                    type == TileID.CorruptGrass || type == TileID.Ebonwood)
                    return false;

                // Crimson
                if (type == TileID.Crimstone || type == TileID.CrimsonGrass ||
                    type == TileID.FleshBlock)
                    return false;

                // Mushroom
                if (type == TileID.MushroomGrass)
                    return false;

                // Dungeon / Temple / Honey
                if (type == TileID.BlueDungeonBrick || type == TileID.GreenDungeonBrick || type == TileID.PinkDungeonBrick ||
                    type == TileID.CrackedBlueDungeonBrick || type == TileID.CrackedGreenDungeonBrick || type == TileID.CrackedPinkDungeonBrick ||
                    type == TileID.LihzahrdBrick || type == TileID.HoneyBlock)
                    return false;

                // Calamity biomes auto‑excluded because they use modded tiles
                // Any modded tile = not forest
                if (type >= TileID.Count)
                    return false;
            }
        }

        return true;
    }

    public override void PostWorldGen()
    {
        List<string> chestList = new(ForestChestSet);
        Shuffle(chestList);

        int placed = 0;
        int attempts = 0;
        int maxAttempts = 20000;

        while (placed < chestList.Count && attempts < maxAttempts)
        {
            attempts++;

            int x = WorldGen.genRand.Next(200, Main.maxTilesX - 200);

            // ORIGINAL underground spawning restored
            int yMin = (int)Main.worldSurface + 10;
            int yMax = (int)Main.rockLayer - 20;
            int y = WorldGen.genRand.Next(yMin, yMax);

            if (!IsForest(x, y))
                continue;

            Generator.GenerateStructure(chestList[placed], new Point16(x, y), Mod);
            placed++;
        }

        if (attempts >= maxAttempts)
            Main.NewText("Forest chest generation aborted early (no valid forest tiles found).");
    }
}