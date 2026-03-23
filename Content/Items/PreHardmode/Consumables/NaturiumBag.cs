using NaturiumMod.Content.Items.PreHardmode.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.PreHardmode.Consumables
{
    public class NaturiumBag : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Consumables/NaturiumBag";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 5;
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.maxStack = 999;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.buyPrice(silver: 45);

            Item.consumable = true;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.UseSound = SoundID.Grab;
        }

        public override bool CanRightClick() => true;

        public override void RightClick(Player player)
        {
            // Drop 10–30 Naturium Ore
            int amount = Main.rand.Next(10, 31);
            player.QuickSpawnItem(player.GetSource_OpenItem(Type),
                ModContent.ItemType<NaturiumOre>(), amount);
        }
    }
    public class NaturiumBarBag : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Consumables/NaturiumBag";
        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 5;
        }

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.maxStack = 999;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.buyPrice(silver: 45);

            Item.consumable = true;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.UseSound = SoundID.Grab;
        }

        public override bool CanRightClick() => true;

        public override void RightClick(Player player)
        {
            // Drop 10–30 Naturium Ore
            int amount = Main.rand.Next(10, 31);
            player.QuickSpawnItem(player.GetSource_OpenItem(Type),
                ModContent.ItemType<NaturiumBar>(), amount);
        }
    }
    public class NaturiumBagDrops : GlobalNPC
    {
        public override void OnKill(NPC npc)
        {
            // Only handle EoW here
            if (npc.type == NPCID.EaterofWorldsHead ||
                npc.type == NPCID.EaterofWorldsBody ||
                npc.type == NPCID.EaterofWorldsTail)
            {
                int segmentCount =
                    NPC.CountNPCS(NPCID.EaterofWorldsHead) +
                    NPC.CountNPCS(NPCID.EaterofWorldsBody) +
                    NPC.CountNPCS(NPCID.EaterofWorldsTail);

                // This OnKill is for the last remaining segment
                if (segmentCount == 1)
                {
                    int mult = Main.masterMode ? 2 : 1;

                    Item.NewItem(
                        npc.GetSource_Loot(),
                        npc.getRect(),
                        ModContent.ItemType<NaturiumBag>(),
                        1 * mult
                    );
                }
            }
        }


        public override bool InstancePerEntity => false;
        
        // -----------------------------
        // BOSS TIERS
        // -----------------------------

        // Early pre-HM bosses (1 bag)
        static readonly int[] EarlyPHMBosses =
        {
        NPCID.EyeofCthulhu,
        NPCID.BrainofCthulhu,
        NPCID.EaterofWorldsHead,   // ADD THIS

        NPCID.QueenBee,
        NPCID.SkeletronHead
    };

        // Later pre-HM bosses (2 bags)
        static readonly int[] LatePHMBosses =
        {
        NPCID.WallofFlesh,
        NPCID.Deerclops
    };

        // Early HM bosses (1 bar bag)
        static readonly int[] EarlyHMBosses =
        {
        NPCID.Retinazer,
        NPCID.Spazmatism,
        NPCID.TheDestroyer,
        NPCID.SkeletronPrime
    };

        // Later HM bosses (2 bar bags)
        static readonly int[] LateHMBosses =
        {
        NPCID.Plantera,
        NPCID.DukeFishron

    };

        // Golem and beyond (3 bar bags)
        static readonly int[] EndgameBosses =
        {
        NPCID.Golem,
        NPCID.HallowBoss,     // Empress of Light
        NPCID.CultistBoss,
        NPCID.MoonLordCore
    };

        // -----------------------------
        // LOOT LOGIC
        // -----------------------------
        
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (!npc.boss)
                return;


            // Master Mode multiplier
            int mult = Main.masterMode ? 2 : 1;

            // -----------------------------
            // PRE-HARDMODE BOSSES
            // -----------------------------
            if (EarlyPHMBosses.Contains(npc.type))
            {
                npcLoot.Add(ItemDropRule.Common(
                    ModContent.ItemType<NaturiumBag>(),
                    1, 1 * mult, 1 * mult
                ));
                return;
            }

            if (LatePHMBosses.Contains(npc.type))
            {
                npcLoot.Add(ItemDropRule.Common(
                    ModContent.ItemType<NaturiumBag>(),
                    1, 2 * mult, 2 * mult
                ));
                return;
            }

            // -----------------------------
            // HARDMODE BOSSES
            // -----------------------------
            if (EarlyHMBosses.Contains(npc.type))
            {
                npcLoot.Add(ItemDropRule.Common(
                    ModContent.ItemType<NaturiumBarBag>(),
                    1, 1 * mult, 1 * mult
                ));
                return;
            }

            if (LateHMBosses.Contains(npc.type))
            {
                npcLoot.Add(ItemDropRule.Common(
                    ModContent.ItemType<NaturiumBarBag>(),
                    1, 2 * mult, 2 * mult
                ));
                return;
            }

            // -----------------------------
            // ENDGAME BOSSES (Golem+)
            // -----------------------------
            if (EndgameBosses.Contains(npc.type))
            {
                npcLoot.Add(ItemDropRule.Common(
                    ModContent.ItemType<NaturiumBarBag>(),
                    1, 3 * mult, 3 * mult
                ));
                return;
            }

            // -----------------------------
            // FALLBACK FOR MODDED BOSSES
            // -----------------------------
            npcLoot.Add(ItemDropRule.Common(
                ModContent.ItemType<NaturiumBarBag>(),
                1, 2 * mult, 2 * mult
            ));
        }
    }
}
