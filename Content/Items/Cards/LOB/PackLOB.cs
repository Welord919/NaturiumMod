using Microsoft.Xna.Framework;
using NaturiumMod.Content.Items.Cards.Fusion;
using NaturiumMod.Content.Items.Cards.LOB.Commons;
using NaturiumMod.Content.Items.Cards.LOB.CommonShortPrint;
using NaturiumMod.Content.Items.Cards.LOB.Rares;
using NaturiumMod.Content.Items.Cards.LOB.SuperRares;
using NaturiumMod.Content.Items.Cards.LOB.UltraRares;
using NaturiumMod.Content.Items.PreHardmode.Consumables;
using NaturiumMod.Content.Items.PreHardmode.Materials;
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
    //  CARD POOLS (ALL RARITIES)
    // ============================================================
    public static class CardPools
    {
        public static readonly List<int> Commons = new()
        {
            ModContent.ItemType<Firegrass>(),
            ModContent.ItemType<AquaMador>(),
            ModContent.ItemType<CelticGuardian>(),
            ModContent.ItemType<SilverFang>(),
            ModContent.ItemType<FlameManipulator>(),
            ModContent.ItemType<Armaill>(),
            ModContent.ItemType<DarkworldThorns>(),
            ModContent.ItemType<Dissolverock>(),
            ModContent.ItemType<Hinotama>(),
            ModContent.ItemType<LesserDragon>(),
            ModContent.ItemType<MonsterEgg>(),
            ModContent.ItemType<OneEyedSD>(),
            ModContent.ItemType<SkullServant>(),
            ModContent.ItemType<SteelOgre>(),
            ModContent.ItemType<MWarrior1>(),
            ModContent.ItemType<MWarrior2>(),
            ModContent.ItemType<MysticalSheep2>(),
        };

        public static readonly List<int> Rares = new()
        {
            ModContent.ItemType<Swords>(),
            ModContent.ItemType<ManEaterBug>(),
            ModContent.ItemType<Masaki>(),
            ModContent.ItemType<CurseofDragon>(),
        };

        public static readonly List<int> ShortPrints = new()
        {
            ModContent.ItemType<PetiteDragon>(),
            ModContent.ItemType<PetiteAngel>(),
            ModContent.ItemType<Polymerization>(),
        };

        public static readonly List<int> SuperRares = new()
        {
            ModContent.ItemType<TriHornedDragon>(),
            ModContent.ItemType<Gaia>(),
            ModContent.ItemType<Exodia>(),
        };

        public static readonly List<int> ExodiaPieces = new()
        {
            ModContent.ItemType<LeftLeg>(),
            ModContent.ItemType<LeftArm>(),
            ModContent.ItemType<RightArm>(),
            ModContent.ItemType<RightLeg>(),
        };

        public static readonly List<int> UltraRares = new()
        {
            ModContent.ItemType<BEWD>(),
            ModContent.ItemType<REBD>(),
            ModContent.ItemType<DarkMagician>(),
        };
    }

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

    // COMMON PACK — 0.1% Ultra
    public class PackLOB_Common : BasePackLOB
    {
        public override int PackValue => 50;

        public override (int weight, List<int> pool, Rarity rarity)[] Rolls => new[]
        {
            (1, CardPools.UltraRares, Rarity.UltraRare),       // 0.1%
            (400, CardPools.ExodiaPieces, Rarity.SuperRare),   //4%
            (800, CardPools.ShortPrints, Rarity.ShortPrint),   // 8%
            (1800, CardPools.Rares, Rarity.Rare),              // 18%
            (6999, CardPools.Commons, Rarity.Common)           // 69.99%
        };
    }

    // RARE PACK — 1% Ultra
    public class PackLOB_Rare : BasePackLOB
    {
        public override int PackValue => 75;

        public override (int weight, List<int> pool, Rarity rarity)[] Rolls => new[]
        {
            (100, CardPools.UltraRares, Rarity.UltraRare),     // 1%
            (400, CardPools.ExodiaPieces, Rarity.SuperRare),  //4%
            (700, CardPools.SuperRares, Rarity.SuperRare),    // 7%
            (1400, CardPools.ShortPrints, Rarity.ShortPrint),  // 14%
            (5000, CardPools.Rares, Rarity.Rare),              // 50%
            (2400, CardPools.Commons, Rarity.Common)           // 24%
        };
    }

    // SUPER PACK
    public class PackLOB_Super : BasePackLOB
    {
        public override int PackValue => 150;

        public override (int weight, List<int> pool, Rarity rarity)[] Rolls => new[]
        {
            (500, CardPools.UltraRares, Rarity.UltraRare),     // 5%
            (6000, CardPools.SuperRares, Rarity.SuperRare),    // 60%
            (2500, CardPools.Rares, Rarity.Rare),              // 25%
            (1000, CardPools.ShortPrints, Rarity.ShortPrint)   // 10%
        };
    }

    // ULTRA PACK — ONLY Ultra + Super
    public class PackLOB_Ultra : BasePackLOB
    {
        public override int PackValue => 300;

        public override (int weight, List<int> pool, Rarity rarity)[] Rolls => new[]
        {
            (70, CardPools.UltraRares, Rarity.UltraRare),       // 70%
            (30, CardPools.SuperRares, Rarity.SuperRare)        // 30%
        };
    }

    // ============================================================
    //  NPC DROP RULES (ALL PACKS)
    // ============================================================
    public class LOBPackDrop : GlobalNPC
    {
        public override void OnKill(NPC npc)
        {
            // Handle Eater of Worlds separately
            if (npc.type == NPCID.EaterofWorldsHead ||
                npc.type == NPCID.EaterofWorldsBody ||
                npc.type == NPCID.EaterofWorldsTail)
            {
                int segmentCount =
                    NPC.CountNPCS(NPCID.EaterofWorldsHead) +
                    NPC.CountNPCS(NPCID.EaterofWorldsBody) +
                    NPC.CountNPCS(NPCID.EaterofWorldsTail);

                // Last segment
                if (segmentCount == 1)
                {
                    // Early boss → Rare guaranteed
                    Item.NewItem(npc.GetSource_Loot(), npc.getRect(),
                        ModContent.ItemType<PackLOB_Rare>(), 1);

                    // Optional: add Super/Ultra chances
                    Item.NewItem(npc.GetSource_Loot(), npc.getRect(),
                        ModContent.ItemType<PackLOB_Super>(), 1, false, 0, true);
                }
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