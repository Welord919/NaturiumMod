using Terraria;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace NaturiumMod.Content.Generation.Ores;

public class NaturiumOrePass(string name, float loadWeight) : GenPass(name, loadWeight)
{
    protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
    {
        progress.Message = NaturiumOreSystem.ExampleOrePassMessage.Value;

        // Ores are quite simple, we simply use a for loop and the WorldGen.TileRunner to place splotches of the specified Tile in the world. // "6E-05" is "scientific notation". It simply means 0.00006 but in some ways is easier to read. (I changed it lul)
        for (int k = 0; k < (int)(Main.maxTilesX * Main.maxTilesY * 6E-05); k++)
        {
            int x = WorldGen.genRand.Next(0, Main.maxTilesX);
            int y = WorldGen.genRand.Next((int)GenVars.worldSurfaceLow, Main.maxTilesY);

            // Then, we call WorldGen.TileRunner with random "strength" and random "steps", as well as the Tile we wish to place. // Feel free to experiment with strength and step to see the shape they generate.
            WorldGen.TileRunner(x, y, WorldGen.genRand.Next(3, 6), WorldGen.genRand.Next(2, 6), ModContent.TileType<Content.Tiles.NaturiumOreTile>());

            // Alternately, we could check the tile already present in the coordinate we are interested.
            // Wrapping WorldGen.TileRunner in the following condition would make the ore only generate in Snow.
            // Tile tile = Framing.GetTileSafely(x, y);
            // if (tile.HasTile && tile.TileType == TileID.SnowBlock) {
            // 	WorldGen.TileRunner(.....);
            // }
        }
    }
}