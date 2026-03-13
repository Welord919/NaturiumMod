using Microsoft.Xna.Framework;
using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.PreHardmode.Cards.Rares;
using NaturiumMod.Content.Items.PreHardmode.Cards.SuperRares;
using NaturiumMod.Content.Items.PreHardmode.Cards.UltraRares;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.PreHardmode.Cards
{
    public class PackLOB : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Cards/PackLOB";

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

            // -------------------------
            // COMMON (60% total)
            // -------------------------
            if (roll < 60)
            {
                int commonRoll = Main.rand.Next(3);

                switch (commonRoll)
                {
                    case 0:
                        player.QuickSpawnItem(player.GetSource_OpenItem(Item.type),
                            ModContent.ItemType<AquaMador>());
                        break;

                    case 1:
                        player.QuickSpawnItem(player.GetSource_OpenItem(Item.type),
                            ModContent.ItemType<CelticGuardian>());
                        break;

                    case 2:
                        player.QuickSpawnItem(player.GetSource_OpenItem(Item.type),
                            ModContent.ItemType<SilverFang>());
                        break;
                }

                return;
            }

            // -------------------------
            // RARE (25% total)
            // -------------------------
            if (roll < 85)
            {
                int rareRoll = Main.rand.Next(2);

                switch (rareRoll)
                {
                    case 0:
                        player.QuickSpawnItem(player.GetSource_OpenItem(Item.type),
                            ModContent.ItemType<Gaia>());
                        break;

                    case 1:
                        player.QuickSpawnItem(player.GetSource_OpenItem(Item.type),
                            ModContent.ItemType<ManEaterBug>());
                        break;
                }

                return;
            }

            // -------------------------
            // SUPER RARE (10%)
            // -------------------------
            if (roll < 95)
            {
                player.QuickSpawnItem(player.GetSource_OpenItem(Item.type),
                    ModContent.ItemType<FlameSwordsman>());
                return;
            }

            // -------------------------
            // ULTRA RARE (5%)
            // -------------------------
            player.QuickSpawnItem(player.GetSource_OpenItem(Item.type),
                ModContent.ItemType<BEWD>());
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
