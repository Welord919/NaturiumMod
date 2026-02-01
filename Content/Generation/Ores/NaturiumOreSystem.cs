using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Terraria.Localization;

namespace NaturiumMod.Content.Generation.Ores;

public class NaturiumOreSystem : ModSystem
{
    public static LocalizedText ExampleOrePassMessage { get; private set; }

    public override void SetStaticDefaults()
    {
        ExampleOrePassMessage = Mod.GetLocalization($"WorldGen.{nameof(ExampleOrePassMessage)}");
    }

    // World generation is explained more in https://github.com/tModLoader/tModLoader/wiki/World-Generation
    public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
    {
        // Because world generation is like layering several images on top of each other, we need to do some steps between the original world generation steps.

        // Most vanilla ores are generated in a step called "Shinies", so for maximum compatibility, we will also do this.
        // First, we find out which step "Shinies" is.
        int shiniesPassIndex = tasks.FindIndex(genpass => genpass.Name.Equals("Shinies"));

        if (shiniesPassIndex != -1)
        {
            // Next, we insert our pass directly after the original "Shinies" pass.
            tasks.Insert(shiniesPassIndex + 1, new NaturiumOrePass("Naturium Ores", 237.4298f));
        }
    }
}

/*public class WorldGenStuffs : ModSystem
{
    public  void ModifyWorldGenTasks(List<GenPass> tasks, ref float totalWeight)
    {
        int shiniesIndex = tasks.FindIndex(x => x.Name.Equals("Shinies"));
        if (shiniesIndex != -1)
        {
            tasks.Insert(shiniesIndex + 1, new PassLegacy("W Ore Generation", OreGeneration));
        }
        //int buriedChestIndex = tasks.FindIndex(x => x.Name.Equals("Buried Chests"));
        //if (buriedChestIndex != -1)
        //{
        //    tasks.Insert(buriedChestIndex + 1, new PassLegacy("TMMC Chest Generation", ChestGeneration));
        //}
    }

    private void OreGeneration(GenerationProgress progress)
    {
        progress.Message = "ModdignGang Mod Ores";

        for (int i = 0; i < (int)((Main.maxTilesX * Main.maxTilesY) * 7E-04); i++)
        {
            int x = WorldGen.genRand.Next(0, Main.maxTilesX);
            int y = WorldGen.genRand.Next((int)WorldGen.worldSurfaceLow, Main.maxTilesY);

            Tile tile = Framing.GetTileSafely(x, y);
            if (tile.active() && (tile.type == TileID.Granite || tile.type == TileID.Marble || tile.type == TileID.Ash))
            {
                WorldGen.TileRunner(x, y, WorldGen.genRand.Next(2, 7), WorldGen.genRand.Next(1, 5), TileType<Tiles.NaturiumOreTile>());
            }
        }
    }

     private void ChestGeneration(GenerationProgress progress)
    {
        progress.Message = "TMMC Mod Chests";
        for (int i = 0; i < 3; i++)
        {
            bool placeSuccessful = false;
            ushort tileToPlace = (ushort)TileType<Tiles.Furniture.TMMCChest>();
            int oldChestId = -1;
            int chestId = -1;
            while (!placeSuccessful)
            {
                int x = WorldGen.genRand.Next(0, Main.maxTilesX);
                int y = WorldGen.genRand.Next(0, Main.maxTilesY);
                oldChestId = chestId;
                chestId = WorldGen.PlaceChest(x, y, tileToPlace, true, 1);
                if (chestId != -1)
                {
                    progress.Message = chestId.ToString();
                    Chest chest = Main.chest[chestId];
                    chest.item[0].SetDefaults(ItemType<Items.Placeables.Special.TMMCBar>());
                    chest.item[0].stack = WorldGen.genRand.Next(5, 20);
                    chest.item[1].SetDefaults(ItemType<Items.Placeables.Special.TMMCSpecialBar>());
                    chest.item[1].stack = WorldGen.genRand.Next(5, 15);
                    int index = 3;
                    switch (i)
                    {
                        case 0:
                            chest.item[2].SetDefaults(ItemID.BandofRegeneration);
                            break;
                        case 1:
                            chest.item[2].SetDefaults(ItemID.HermesBoots);
                            break;
                        default:
                            chest.item[2].SetDefaults(ItemID.MagicMirror);
                            break;
                    }

                    if (WorldGen.genRand.Next(3) == 0)
                    {
                        chest.item[index].SetDefaults(ItemID.Bomb);
                        chest.item[index].stack = WorldGen.genRand.Next(10, 20);
                        index++;
                    }
                    if (WorldGen.genRand.Next(2) == 0)
                    {
                        chest.item[index].SetDefaults(ItemID.Shuriken);
                        chest.item[index].stack = WorldGen.genRand.Next(30, 50);
                        index++;
                    }
                    TMMCUtils.Log("Chest at {0}, {1}", x, y);
                    placeSuccessful = true;
                }
            }
        }

    }*/
