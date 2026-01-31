using NaturiumMod.Content.NPCs;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace NaturiumMod.Content.Tiles;

// ExampleStatue shows off correctly using wiring to spawn items and NPC.
// See StatueWorldGen to see how ExampleStatue is added as an option for naturally spawning statues during world-gen.
public class NatSlimeStatueTile : ModTile
{
    public override string Texture => "NaturiumMod/Assets/Tiles/NatSlimeStatueTile";

    public override void SetStaticDefaults()
    {
        Main.tileFrameImportant[Type] = true;
        Main.tileObsidianKill[Type] = true;
        TileID.Sets.DisableSmartCursor[Type] = true;
        TileID.Sets.IsAMechanism[Type] = true; // Ensures that this tile and connected pressure plate won't be removed during the "Remove Broken Traps" world-gen step

        TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
        TileObjectData.addTile(Type);
    }

    // This hook allows you to make anything happen when this statue is powered by wiring.
    // In this example, powering the statue either spawns a random coin with a 95% chance, or, with a 5% chance - a goldfish.
    public override void HitWire(int i, int j)
    {
        Tile tile = Main.tile[i, j];

        // Find the coordinates of top left tile square through math
        int yCoordinate = j - tile.TileFrameY / 18;
        int xCoordinate = i - tile.TileFrameX / 18;

        const int TileWidth = 2;
        const int TileHeight = 3;

        // Here we call SkipWire on all tile coordinates covered by this tile. This ensures a wire signal won't run multiple times.
        for (int y = yCoordinate; y < yCoordinate + TileHeight; y++)
        {
            for (int x = xCoordinate; x < xCoordinate + TileWidth; x++)
            {
                Wiring.SkipWire(x, y);
            }
        }

        // Calculate the center of this tile to use as an entity spawning position.
        // Note that we use 0.65 for height because even though the statue takes 3 blocks, its appearance is shorter.
        float spawnX = (xCoordinate + TileWidth * 0.5f) * 16;
        float spawnY = (yCoordinate + TileHeight * 0.65f) * 16;

        // This example shows both item spawning code and npc spawning code, you can use whichever code suits your mod
        // There is a 95% chance for item spawn and a 5% chance for npc spawn
        // If you want to make a item spawning statue, see below.

        var entitySource = new EntitySource_TileUpdate(xCoordinate, yCoordinate, context: "NatSlimeStatue");

        // If you want to make an NPC spawning statue, see below.
        int npcIndex = -1;

        // 30 is the time before it can be used again. NPC.MechSpawn checks nearby for other spawns to prevent too many spawns. 3 in immediate vicinity, 6 nearby, 10 in world.
        int spawnedNpcId = ModContent.NPCType<NaturiumSlime>();

        if (Wiring.CheckMech(xCoordinate, yCoordinate, 30) && NPC.MechSpawn(spawnX, spawnY, spawnedNpcId))
        {
            npcIndex = NPC.NewNPC(entitySource, (int)spawnX, (int)spawnY - 12, spawnedNpcId);
        }

        if (npcIndex >= 0)
        {
            var npc = Main.npc[npcIndex];

            npc.value = 0f;
            npc.npcSlots = 0f;
            // Prevents Loot if NPCID.Sets.NoEarlymodeLootWhenSpawnedFromStatue and !Main.HardMode or NPCID.Sets.StatueSpawnedDropRarity != -1 and NextFloat() >= NPCID.Sets.StatueSpawnedDropRarity or killed by traps.
            // Prevents CatchNPC
            npc.SpawnedFromStatue = true;
        }
    }
}
