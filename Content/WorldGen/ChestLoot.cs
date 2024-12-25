using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using ModdingGang.Content.Items.Materials;

namespace ModdingGang.Content.WorldGen
{
    public class ChestItemWorldGen : ModSystem
    {
        private void AddItemsToChest<T>(int chestType, int minimumOre, int maximumOre, float chanceToSpawn) where T : ModItem
        {
            for (int chestIndex = 0; chestIndex < 1000; chestIndex++)
            {
                Chest chest = Main.chest[chestIndex];
                if (chest == null || Main.tile[chest.x, chest.y].TileType != TileID.Containers || Main.tile[chest.x, chest.y].TileFrameX != chestType * 36 || Main.rand.NextFloat() >= chanceToSpawn)
                {
                    continue;
                }

                int stackNum = Main.rand.Next(minimumOre, maximumOre);
                int itemType = ModContent.ItemType<T>();

                for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                {
                    if (chest.item[inventoryIndex].type == ItemID.None)
                    {
                        chest.item[inventoryIndex].SetDefaults(itemType);
                        chest.item[inventoryIndex].stack = stackNum;
                        break;
                    }
                }
            }
        }

        public override void PostWorldGen()
        {
            //Wooden with Barkions Bark
            AddItemsToChest<BarkionsBark>(0, 13, 26, 0.09f);

            //Golden with Naturium Ore
            AddItemsToChest<NaturiumOre>(1, 5, 30, 0.07f);

            //Living with Barkions Bark
            AddItemsToChest<BarkionsBark>(12, 12, 48, 0.07f);

            //Living with Naturium Ore
            AddItemsToChest<NaturiumOre>(12, 5, 30, 0.02f);

            //Jungle with Naturium Ore
            AddItemsToChest<NaturiumOre>(8, 3, 25, 0.09f);

            //Ivy with Naturium Ore
            AddItemsToChest<NaturiumOre>(10, 10, 50, 0.45f);
        }
    }
}