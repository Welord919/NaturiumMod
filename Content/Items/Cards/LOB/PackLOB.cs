using Microsoft.Xna.Framework;
using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.Cards.LOB.Commons;
using NaturiumMod.Content.Items.Cards.LOB.CommonShortPrint;
using NaturiumMod.Content.Items.Cards.LOB.Rares;
using NaturiumMod.Content.Items.Cards.LOB.SuperRares;
using NaturiumMod.Content.Items.Cards.LOB.UltraRares;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static NaturiumMod.Content.Items.Cards.CardRarityHelper;

namespace NaturiumMod.Content.Items.Cards.LOB
{
    public class PackLOB : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/PackLOB";

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;

            Item.maxStack = 999;
            Item.consumable = true;

            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useAnimation = 20;
            Item.useTime = 20;

            Item.rare = ItemRarityID.Blue;
            Item.UseSound = SoundID.Grab;

            Item.value = Item.buyPrice(gold: 3);
        }

        // ⭐ Allows right‑click opening
        public override bool CanRightClick() => true;

        public override void RightClick(Player player)
        {
            // Weighted roll (0–99)
            int roll = Main.rand.Next(100);

            //Commons (60% total)
            if (roll < 60)
            {

                int commonRoll = Main.rand.Next(4);

                switch (commonRoll)
                {
                    case 0:
                        player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<AquaMador>());
                        AnnounceCard(player, "Aqua Mador", Rarity.Common);

                        break;

                    case 1:
                        player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<CelticGuardian>());
                        AnnounceCard(player, "Celtic Guardian", Rarity.Common);

                        break;

                    case 2:
                        player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<SilverFang>());
                        AnnounceCard(player, "Silver Fang", Rarity.Common);

                        break;
                    case 3:
                        player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<FlameManipulator>());
                        AnnounceCard(player, "Flame Manipulator", Rarity.Common);

                        break;
                }

                return;
            }

            //Rare (20% total)
            if (roll < 80)
            {
                int rareRoll = Main.rand.Next(4);

                switch (rareRoll)
                {
                    case 0:
                        player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<Gaia>());
                        AnnounceCard(player, "Gaia the Fierce Knight", Rarity.Rare);

                        break;

                    case 1:
                        player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<ManEaterBug>());
                        AnnounceCard(player, "Man-Eater Bug", Rarity.Rare);

                        break;
                    case 2:
                        player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<Masaki>());
                        AnnounceCard(player, "Masaki", Rarity.Rare);

                        break;
                    case 3:
                        player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<CurseofDragon>());
                        AnnounceCard(player, "Curse of Dragon", Rarity.Rare);

                        break;
                }

                return;
            }

            //Short Print Commons (10% total)
            if (roll < 90)
            {
                int CSPRoll = Main.rand.Next(2);

                switch (CSPRoll)
                {
                    case 0:
                        player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<PetiteDragon>());
                        AnnounceCard(player, "Petite Dragon", Rarity.CommonSP);

                        break;
                    case 1:
                        player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<PetiteDragon>());
                        AnnounceCard(player, "Petite Dragon", Rarity.CommonSP);

                        break;
                }

                return;
            }
            //Super Rares (6% total)
            if (roll < 96)
            {
                int rareRoll = Main.rand.Next(3);

                switch (rareRoll)
                {
                    case 0:
                        player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<FlameSwordsman>());
                        AnnounceCard(player, "Flame Swordsman", Rarity.SuperRare);

                        break;

                    case 1:
                        player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<LeftLeg>());
                        AnnounceCard(player, "Left Leg of Exodia", Rarity.SuperRare);

                        break;
                    case 2:
                        player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<Swords>());
                        AnnounceCard(player, "Swords of revealing light", Rarity.SuperRare);

                        break;
                }

                return;
            }
            //Super Rares (4% total)
            if (roll < 100)
            {
                int rareRoll = Main.rand.Next(3);

                switch (rareRoll)
                {
                    case 0:
                        player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<BEWD>());
                        AnnounceCard(player, "Blue Eyes White Dragon", Rarity.UltraRare);

                        break;

                    case 1:
                        player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<REBD>());
                        AnnounceCard(player, "Red Eyes Black Dragon", Rarity.UltraRare);

                        break;
                    case 2:
                        player.QuickSpawnItem(player.GetSource_OpenItem(Item.type), ModContent.ItemType<DarkMagician>());
                        AnnounceCard(player, "Dark Magician", Rarity.UltraRare);

                        break;
                }

                return;
            }

        }

    }

    // ⭐ Global NPC drop for all basic enemies
    public class LOBPackDrop : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (!ModContent.GetInstance<NaturiumConfig>().CardDrops)
                return;


            // Only drop from basic enemies (no bosses, no town NPCs)
            if (!npc.friendly && npc.lifeMax > 5 && npc.damage > 0 && !npc.boss)
            {
                npcLoot.Add(ItemDropRule.Common(
                    ModContent.ItemType<PackLOB>(),
                    20 // 1 in 20 drop chance (5%)
                ));
            }
        }
    }
}
