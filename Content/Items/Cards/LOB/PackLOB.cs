using Microsoft.Xna.Framework;
using NaturiumMod.Content.Items.Cards.Fusion;
using NaturiumMod.Content.Items.Cards.LOB.Commons;
using NaturiumMod.Content.Items.Cards.LOB.CommonShortPrint;
using NaturiumMod.Content.Items.Cards.LOB.NoEffect;
using NaturiumMod.Content.Items.Cards.LOB.Rares;
using NaturiumMod.Content.Items.Cards.LOB.SuperRares;
using NaturiumMod.Content.Items.Cards.LOB.UltraRares;
using System.Collections.Generic;
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
            ModContent.ItemType<Polymerization>(),
        };

        public static readonly List<int> SuperRares = new()
        {
            ModContent.ItemType<TriHornedDragon>(),
            ModContent.ItemType<LeftLeg>(),
            ModContent.ItemType<Gaia>(),
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
            (2000, CardPools.Rares, Rarity.Rare),              // 20%
            (1000, CardPools.ShortPrints, Rarity.ShortPrint),  // 10%
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
            (1000, CardPools.SuperRares, Rarity.SuperRare),    // 10%
            (1500, CardPools.ShortPrints, Rarity.ShortPrint),  // 15%
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
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (!npc.friendly && npc.lifeMax > 5 && npc.damage > 0 &&
                !npc.boss && !npc.SpawnedFromStatue && npc.realLife == -1)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PackLOB_Common>(), 20));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PackLOB_Rare>(), 40));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PackLOB_Super>(), 80));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PackLOB_Ultra>(), 200));
            }
        }
    }
}