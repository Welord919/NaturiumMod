using System.Collections.Generic;
using NaturiumMod.Content.Items.PreHardmode.Materials;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Generation;

public class ChestItemWorldGen : ModSystem
{
    public override void PostWorldGen()
    {
        ChestInfo[] chests = GetGeneratedChests();

        foreach (ChestInfo chest in chests)
        {
            switch (chest.Type)
            {
                case ChestType.Wooden:
                    chest.TryAddItem<BarkionsBark>(13, 26, 0.09f);
                    break;
                case ChestType.Gold:
                    chest.TryAddItem<NaturiumOre>(5, 30, 0.07f);
                    break;
                case ChestType.LivingWood:
                    chest.TryAddItem<BarkionsBark>(12, 48, 0.07f);
                    chest.TryAddItem<NaturiumOre>(5, 30, 0.02f);
                    break;
                case ChestType.Jungle:
                    chest.TryAddItem<NaturiumOre>(3, 25, 0.09f);
                    break;
                case ChestType.Ivy:
                    chest.TryAddItem<NaturiumOre>(10, 50, 0.45f);
                    break;
            }
        }
    }

    private static int GetChestDropItemType(Tile tile)
    {
        int style = tile.TileFrameX / 36;

        return tile.TileType switch
        {
            TileID.Containers when (uint)style < (uint)Chest.chestItemSpawn.Length =>
                Chest.chestItemSpawn[style],
            TileID.Containers2 when (uint)style < (uint)Chest.chestItemSpawn2.Length =>
                Chest.chestItemSpawn2[style],
            _ => ItemID.None
        };
    }

    private static ChestInfo[] GetGeneratedChests()
    {
        List<ChestInfo> result = [];

        for (int i = 0; i < Main.maxChests; i++)
        {
            Chest chest = Main.chest[i];
            if (chest is null)
            {
                continue;
            }

            Tile tile = Framing.GetTileSafely(chest.x, chest.y);
            if (tile.TileType != TileID.Containers && tile.TileType != TileID.Containers2)
            {
                continue;
            }

            int dropItemID = GetChestDropItemType(tile);
            if (dropItemID == ItemID.None)
            {
                continue;
            }

            result.Add(new ChestInfo(chest, dropItemID));
        }

        return [.. result];
    }
}
