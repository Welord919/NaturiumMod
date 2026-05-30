using Microsoft.Xna.Framework;
using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.PreHardmode.MillenniumItems;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

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
            CardRarityHelper.AnnounceCard(player, Lang.GetItemNameValue(item), rarity);
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
            Item.noUseGraphic = true;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.UseSound = SoundID.Grab;
            Item.value = Item.buyPrice(silver: PackValue);
            ItemTags.AddTagToItem(Type, "Card");
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
        public override bool InstancePerEntity => false;

        // ============================================================
        // VALID ENEMY CHECK (must be OUTSIDE any method)
        // ============================================================
        private bool IsValidEnemy(NPC npc)
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

        // ============================================================
        // BOSS MINION CHECK (must be OUTSIDE any method)
        // ============================================================
        private bool IsBossMinion(int type)
        {
            return
                type == NPCID.ServantofCthulhu ||
                type == NPCID.Creeper ||
                type == NPCID.Bee ||
                type == NPCID.BeeSmall ||
                type == NPCID.SkeletronHand ||
                type == NPCID.EaterofWorldsHead ||
                type == NPCID.EaterofWorldsBody ||
                type == NPCID.EaterofWorldsTail ||
                type == NPCID.TheDestroyer ||
                type == NPCID.TheDestroyerBody ||
                type == NPCID.TheDestroyerTail ||
                type == NPCID.Probe ||
                type == NPCID.QueenSlimeMinionBlue ||
                type == NPCID.QueenSlimeMinionPink ||
                type == NPCID.QueenSlimeMinionPurple ||
                type == NPCID.TheHungry ||
                type == NPCID.TheHungryII ||
                type == NPCID.LeechHead ||
                type == NPCID.LeechBody ||
                type == NPCID.LeechTail ||
                type == NPCID.PrimeCannon ||
                type == NPCID.PrimeLaser ||
                type == NPCID.PrimeSaw ||
                type == NPCID.PrimeVice ||
                type == NPCID.PlanterasTentacle ||
                type == NPCID.GolemFistLeft ||
                type == NPCID.GolemFistRight ||
                type == NPCID.Sharkron ||
                type == NPCID.Sharkron2 ||
                type == NPCID.CultistDragonHead ||
                type == NPCID.CultistDragonBody1 ||
                type == NPCID.CultistDragonBody2 ||
                type == NPCID.CultistDragonBody3 ||
                type == NPCID.CultistDragonTail ||
                type == NPCID.PumpkingBlade ||
                type == NPCID.MourningWood ||
                type == NPCID.Everscream ||
                type == NPCID.SantaNK1 ||
                type == NPCID.IceQueen ||
                type == NPCID.AncientDoom ||
                type == NPCID.CultistArcherBlue ||
                type == NPCID.CultistArcherWhite ||
                type == NPCID.CultistBossClone ||
                type == NPCID.CultistDevote ||
                type == NPCID.FlyingSnake;
        }

        // ============================================================
        // BOSS DROPS (ModifyNPCLoot)
        // ============================================================
        static readonly int[] EarlyBosses =
        {
        NPCID.EyeofCthulhu,
        NPCID.BrainofCthulhu,
        NPCID.QueenBee,
        NPCID.SkeletronHead
    };

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

        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            if (npc.boss)
            {
                if (EarlyBosses.Contains(npc.type))
                {
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PackLOB_Rare>(), 1));
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PackLOB_Super>(), 5));
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PackLOB_Ultra>(), 15));
                    return;
                }

                if (MidLateBosses.Contains(npc.type))
                {
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PackLOB_Super>(), 1));
                    npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PackLOB_Ultra>(), 10));
                    return;
                }

                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PackLOB_Super>(), 1));
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PackLOB_Ultra>(), 10));
            }
        }

        // ============================================================
        // NORMAL ENEMY PACK DROPS (OnKill)
        // ============================================================
        public override void OnKill(NPC npc)
        {
            if (!IsValidEnemy(npc))
                return;

            Player player = Main.player[npc.lastInteraction];
            float boost = player.GetModPlayer<CardDropPlayer>().CardDropBoost;

            float commonChance = MathHelper.Clamp(PackDropConstants.CommonBase * (1f + boost), 0f, 1f);
            float rareChance = MathHelper.Clamp(PackDropConstants.RareBase * (1f + boost), 0f, 1f);
            float superChance = MathHelper.Clamp(PackDropConstants.SuperBase * (1f + boost), 0f, 1f);
            float ultraChance = MathHelper.Clamp(PackDropConstants.UltraBase * (1f + boost), 0f, 1f);

            if (Main.rand.NextFloat() < commonChance)
                Item.NewItem(npc.GetSource_Loot(), npc.getRect(), ModContent.ItemType<PackLOB_Common>());

            if (Main.rand.NextFloat() < rareChance)
                Item.NewItem(npc.GetSource_Loot(), npc.getRect(), ModContent.ItemType<PackLOB_Rare>());

            if (Main.rand.NextFloat() < superChance)
                Item.NewItem(npc.GetSource_Loot(), npc.getRect(), ModContent.ItemType<PackLOB_Super>());

            if (Main.rand.NextFloat() < ultraChance)
                Item.NewItem(npc.GetSource_Loot(), npc.getRect(), ModContent.ItemType<PackLOB_Ultra>());
        }
    }
    public static class PackDropConstants
    {
        public const float CommonBase = 1f / 20f;
        public const float RareBase = 1f / 40f;
        public const float SuperBase = 1f / 80f;
        public const float UltraBase = 1f / 200f;
    }
    public class CardDropPlayer : ModPlayer
    {
        public float CardDropBoost;

        public override void ResetEffects()
        {
            CardDropBoost = 0f; // reset every tick
        }
    }
}