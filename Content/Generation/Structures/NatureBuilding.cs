using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Generator = StructureHelper.API.Generator;

namespace NaturiumMod.Content.Generation.Structures;

public class NatureBuilding : ModSystem
{
    public override void PostWorldGen()
    {
        for (int i = 0; i < 4; i++)
        {
            int x = WorldGen.genRand.Next(200, Main.maxTilesX - 200);
            int y = WorldGen.genRand.Next((int)Main.rockLayer, Main.maxTilesY - 200);

            Generator.GenerateStructure("Assets/Structures/NatureBuilding", new Point16(x, y), Mod);
        }
    }
}
