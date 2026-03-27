using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Cards.LOB.SuperShortPrint
{
    public class PotofGreed : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/PotofGreed";
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 32;
            Item.maxStack = 99;
            Item.rare = ItemRarityID.Green;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.consumable = true;
            Item.value = Item.buyPrice(silver: 50);
        }

        public override bool? UseItem(Player player)
        {
            // Roll twice
            for (int i = 0; i < 2; i++)
            {
                int pack = RollWeightedPack();
                player.QuickSpawnItem(player.GetSource_FromThis(), pack);
            }

            return true;
        }
        private int RollWeightedPack()
        {
            int roll = Main.rand.Next(100); // 0–99

            if (roll < 50)
                return ModContent.ItemType<PackLOB_Common>();     // 50%

            if (roll < 75)
                return ModContent.ItemType<PackLOB_Rare>();       // 25%

            if (roll < 95)
                return ModContent.ItemType<PackLOB_Super>();      // 20%

            return ModContent.ItemType<PackLOB_Ultra>();          // 5%
        }

    }

}
