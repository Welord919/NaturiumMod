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

                // ❄ Snow / Ice biome
                if (type == TileID.SnowBlock || type == TileID.IceBlock ||
                    type == TileID.CorruptIce || type == TileID.FleshIce)
                    return false;

                // 🌿 Jungle biome
                if (type == TileID.JungleGrass || type == TileID.Mud)
                    return false;

                // 🏜 Desert biome
                if (type == TileID.Sand || type == TileID.HardenedSand || type == TileID.Sandstone || 
                    type == TileID.CorruptHardenedSand || type == TileID.CorruptSandstone ||
                    type == TileID.Crimsand || type == TileID.CrimsonHardenedSand || type == TileID.CrimsonSandstone)
                    return false;

                // 💀 Corruption biome
                if (type == TileID.Ebonstone || type == TileID.EbonstoneBrick ||
                    type == TileID.CorruptGrass || type == TileID.Ebonwood)
                    return false;

                // ❤️ Crimson biome
                if (type == TileID.Crimstone || type == TileID.CrimsonGrass ||
                    type == TileID.FleshBlock)
                    return false;

                // 🍄 Mushroom biome
                if (type == TileID.MushroomGrass)
                    return false;

                // Bricks
                if (type == TileID.BlueDungeonBrick || type == TileID.GreenDungeonBrick || type == TileID.PinkDungeonBrick 
                        || type == TileID.LihzahrdBrick || type == TileID.HoneyBlock || type == TileID.CrackedBlueDungeonBrick 
                        || type == TileID.CrackedGreenDungeonBrick || type == TileID.CrackedPinkDungeonBrick)
                    return false;
            }
        }

        return true; // No biome-defining tiles → Forest
    }


    public override void PostWorldGen()
    {
        // Make a working copy so the original list stays intact
        List<string> chestList = new(ForestChestSet);

        // Shuffle so the order is random each world
        Shuffle(chestList);

        int placed = 0;

        while (placed < chestList.Count)
        {
            int x = WorldGen.genRand.Next(200, Main.maxTilesX - 200);

            int yMin = (int)Main.worldSurface + 10;
            int yMax = (int)Main.rockLayer - 20;

            int y = WorldGen.genRand.Next(yMin, yMax);

            if (!IsForest(x, y))
                continue;

            // Place the next chest structure in the shuffled list
            Generator.GenerateStructure(chestList[placed], new Point16(x, y), Mod);
            placed++;
        }
    }

}
