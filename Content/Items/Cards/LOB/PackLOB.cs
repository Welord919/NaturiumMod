using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using static NaturiumMod.Content.Items.Cards.CardRarityHelper;

namespace NaturiumMod.Content.Items.Cards.LOB
{

    // ============================================================
    //  PACK BUILDER (UNIVERSAL)
    // ============================================================
    public static class PackBuilder
    {
        public static void GiveRandomFromPool(Player player, List<int> pool, Rarity rarity, int source)
        {
            int item = pool[Main.rand.Next(pool.Count)];
            player.QuickSpawnItem(player.GetSource_OpenItem(source), item);
            AnnounceCard(player, Lang.GetItemNameValue(item), rarity);
        }

        public static void RollWeighted(Player player, int source,
            params (int weight, List<int> pool, Rarity rarity)[] entries)
        {
            int total = 0;
            foreach (var e in entries) total += e.weight;

            int roll = Main.rand.Next(total);
            int sum = 0;

            foreach (var e in entries)
            {
                sum += e.weight;
                if (roll < sum)
                {
                    GiveRandomFromPool(player, e.pool, e.rarity, source);
                    return;
                }
            }
        }
    }

    // ============================================================
    //  BASE PACK CLASS (ALL PACKS INHERIT THIS)
    // ============================================================
    public abstract class BasePackLOB : ModItem
    {
        public abstract (int weight, List<int> pool, Rarity rarity)[] Rolls { get; }
        public abstract int PackValue { get; }

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
            Item.UseSound = SoundID.Grab;
            Item.value = Item.buyPrice(silver: PackValue);
        }

        public override bool CanRightClick() => true;

        public override void RightClick(Player player)
        {
            PackBuilder.RollWeighted(player, Item.type, Rolls);
        }
    }

    // ============================================================
    //  PACK TYPES
    // ============================================================

    // COMMON PACK
    public class PackLOB_Common : BasePackLOB
    {
        public override int PackValue => 50;

        public override (int weight, List<int> pool, Rarity rarity)[] Rolls => new[]
        {
        (1,    CardPools.GetPool(Rarity.UltraRare).ToList(),    Rarity.UltraRare),
        (100,  CardPools.GetPool(Rarity.SuperRare).ToList(),    Rarity.SuperRare),
        (400,  CardPools.GetPool(Rarity.Exodia).ToList(),       Rarity.Exodia),
        (800,  CardPools.GetPool(Rarity.ShortPrint).ToList(),   Rarity.ShortPrint),
        (1700, CardPools.GetPool(Rarity.Rare).ToList(),         Rarity.Rare),
        (6999, CardPools.GetPool(Rarity.Common).ToList(),       Rarity.Common)
    };
    }

    // RARE PACK
    public class PackLOB_Rare : BasePackLOB
    {
        public override int PackValue => 75;

        public override (int weight, List<int> pool, Rarity rarity)[] Rolls => new[]
        {
        (100,  CardPools.GetPool(Rarity.UltraRare).ToList(),    Rarity.UltraRare),
        (400,  CardPools.GetPool(Rarity.Exodia).ToList(),       Rarity.Exodia),
        (700,  CardPools.GetPool(Rarity.SuperRare).ToList(),    Rarity.SuperRare),
        (1400, CardPools.GetPool(Rarity.ShortPrint).ToList(),   Rarity.ShortPrint),
        (5000, CardPools.GetPool(Rarity.Rare).ToList(),         Rarity.Rare),
        (2400, CardPools.GetPool(Rarity.Common).ToList(),       Rarity.Common)
    };
    }

    // SUPER PACK
    public class PackLOB_Super : BasePackLOB
    {
        public override int PackValue => 150;

        public override (int weight, List<int> pool, Rarity rarity)[] Rolls => new[]
        {
        (500,  CardPools.GetPool(Rarity.UltraRare).ToList(),    Rarity.UltraRare),
        (6000, CardPools.GetPool(Rarity.SuperRare).ToList(),    Rarity.SuperRare),
        (1500, CardPools.GetPool(Rarity.Rare).ToList(),         Rarity.Rare),
        (1000, CardPools.GetPool(Rarity.ShortPrint).ToList(),   Rarity.ShortPrint),
        (1000, CardPools.GetPool(Rarity.SuperShortPrint).ToList(), Rarity.SuperShortPrint)
    };
    }

    // ULTRA PACK
    public class PackLOB_Ultra : BasePackLOB
    {
        public override int PackValue => 300;

        public override (int weight, List<int> pool, Rarity rarity)[] Rolls => new[]
        {
        (75,  CardPools.GetPool(Rarity.UltraRare).ToList(),        Rarity.UltraRare),
        (19,  CardPools.GetPool(Rarity.SuperRare).ToList(),       Rarity.SuperRare),
        (6,   CardPools.GetPool(Rarity.SuperShortPrint).ToList(), Rarity.SuperShortPrint)
    };
    }


    // ============================================================
    //  NPC DROP RULES (ALL PACKS)
    // ============================================================
    public class LOBPackDrop : GlobalNPC
    {
        public override void OnKill(NPC npc)
        {
            // Handle Eater of Worlds FINAL KILL only
            if (npc.type == NPCID.EaterofWorldsHead ||
                npc.type == NPCID.EaterofWorldsBody ||
                npc.type == NPCID.EaterofWorldsTail)
            {
                int segments =
                    NPC.CountNPCS(NPCID.EaterofWorldsHead) +
                    NPC.CountNPCS(NPCID.EaterofWorldsBody) +
                    NPC.CountNPCS(NPCID.EaterofWorldsTail);

                // Only drop on the FINAL segment
                if (segments == 1)
                {
                    // Rare (100%)
                    Item.NewItem(npc.GetSource_Loot(), npc.getRect(),
                        ModContent.ItemType<PackLOB_Rare>(), 1);

                    // Super (1/5)
                    if (Main.rand.NextBool(5))
                    {
                        Item.NewItem(npc.GetSource_Loot(), npc.getRect(),
                            ModContent.ItemType<PackLOB_Super>(), 1);
                    }

                    // Ultra (1/15)
                    if (Main.rand.NextBool(15))
                    {
                        Item.NewItem(npc.GetSource_Loot(), npc.getRect(),
                            ModContent.ItemType<PackLOB_Ultra>(), 1);
                    }
                }

                return; // Prevent ModifyNPCLoot from running
            }
        }

        public override bool InstancePerEntity => false;

        // Early‑game bosses allowed to drop Rare packs
        static readonly int[] EarlyBosses =
        {
        NPCID.EyeofCthulhu,
        NPCID.BrainofCthulhu,
        NPCID.QueenBee,
        NPCID.SkeletronHead
    };

        // Higher‑tier bosses (SuperRare+ only)
        static readonly int[] MidLateBosses =
        {
        NPCID.WallofFlesh,
        NPCID.Retinazer,
        NPCID.Spazmatism,
        NPCID.TheDestroyer,
        NPCID.SkeletronPrime,
        NPCID.Plantera,
        NPCID.Golem,
        NPCID.DukeFishron,
        NPCID.CultistBoss,
        NPCID.MoonLordCore
    };

        bool IsBossMinion(int type)
        {
            return
                // Eye of Cthulhu
                type == NPCID.ServantofCthulhu ||

                // Brain of Cthulhu
                type == NPCID.Creeper ||

                // Queen Bee
                type == NPCID.Bee ||
                type == NPCID.BeeSmall ||

                // Skeletron
                type == NPCID.SkeletronHand ||

                // Eater of Worlds (all segments)
                type == NPCID.EaterofWorldsHead ||
                type == NPCID.EaterofWorldsBody ||
                type == NPCID.EaterofWorldsTail ||

                // The Destroyer (all segments)
                type == NPCID.TheDestroyer ||
                type == NPCID.TheDestroyerBody ||
                type == NPCID.TheDestroyerTail ||
                type == NPCID.Probe || // FIX: Destroyer probes

                // Queen Slime minions
                type == NPCID.QueenSlimeMinionBlue ||
                type == NPCID.QueenSlimeMinionPink ||
                type == NPCID.QueenSlimeMinionPurple ||

                // Wall of Flesh
                type == NPCID.TheHungry ||
                type == NPCID.TheHungryII ||
                type == NPCID.LeechHead ||
                type == NPCID.LeechBody ||
                type == NPCID.LeechTail ||

                // Skeletron Prime arms
                type == NPCID.PrimeCannon ||
                type == NPCID.PrimeLaser ||
                type == NPCID.PrimeSaw ||
                type == NPCID.PrimeVice ||

                // Plantera
                type == NPCID.PlanterasTentacle ||

                // Golem
                type == NPCID.GolemFistLeft ||
                type == NPCID.GolemFistRight ||

                // Duke Fishron
                type == NPCID.Sharkron ||
                type == NPCID.Sharkron2 ||

                // Lunatic Cultist
                type == NPCID.CultistDragonHead ||
                type == NPCID.CultistDragonBody1 ||
                type == NPCID.CultistDragonBody2 ||
                type == NPCID.CultistDragonBody3 ||
                type == NPCID.CultistDragonTail ||

                // Pumpkin Moon
                type == NPCID.PumpkingBlade ||
                type == NPCID.MourningWood ||

                // Frost Moon
                type == NPCID.Everscream ||
                type == NPCID.SantaNK1 ||
                type == NPCID.IceQueen ||

                // Ancient Doom
                type == NPCID.AncientDoom ||

                //Cultist
                type == NPCID.CultistArcherBlue||
                type == NPCID.CultistArcherWhite ||
                type == NPCID.CultistBossClone ||
                type == NPCID.CultistDevote ||
                type == NPCID.CultistDragonBody1 ||
                type == NPCID.CultistDragonBody2 ||
                type == NPCID.CultistDragonBody3 ||
                type == NPCID.CultistDragonBody4 ||
                type == NPCID.CultistDragonHead ||
                type == NPCID.CultistDragonTail ||

                // Flying Snake
                type == NPCID.FlyingSnake;
        }

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            
            bool IsValidEnemy(NPC npc)
            {
                if (npc.friendly || npc.townNPC || npc.lifeMax <= 5 || npc.damage <= 0)
                    return false;

                if (npc.boss || npc.realLife != -1)
                    return false;

                if (IsBossMinion(npc.type))
                    return false;

                if (npc.SpawnedFromStatue)
                    return false;

                if (NPCID.Sets.BelongsToInvasionOldOnesArmy[npc.type])
                    return false;


                return true;
            }

            // NORMAL ENEMIES
            if (IsValidEnemy(npc))
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PackLOB_Common>(), 20));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PackLOB_Rare>(), 40));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PackLOB_Super>(), 80));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PackLOB_Ultra>(), 200));
                return;
            }

            // ------------------------------------------------------------
            // BOSS DROPS
            // ------------------------------------------------------------
            if (npc.boss)
            {


                // -------------------------
                // EARLY BOSSES
                // -------------------------
                if (EarlyBosses.Contains(npc.type))
                {
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PackLOB_Rare>(), 1));
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PackLOB_Super>(), 5));
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PackLOB_Ultra>(), 15));
                    return;
                }

                // -------------------------
                // MID/LATE BOSSES
                // -------------------------
                if (MidLateBosses.Contains(npc.type))
                {
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PackLOB_Super>(), 1));
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PackLOB_Ultra>(), 10));
                    return;
                }

                // -------------------------
                // MODDED BOSSES (fallback)
                // -------------------------
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PackLOB_Super>(), 1));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PackLOB_Ultra>(), 10));
            }
        }
    }
}