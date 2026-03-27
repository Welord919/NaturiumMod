using System;
using Microsoft.Xna.Framework;
using NaturiumMod.Content.Items.General.Projectiles;
using NaturiumMod.Content.Items.General.Placeable;
using NaturiumMod.Content.Items.PreHardmode.Materials;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using NaturiumMod.Content.Items.PreHardmode.Weapons;

namespace NaturiumMod.Content.NPCs.SpiritReaper
{
    public class SpiritReaper : ModNPC
    {
        public override string Texture => "NaturiumMod/Assets/NPCs/SpiritReaperNPC";

        private int attackTimer;

        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 3;

            // Bestiary drawing tweaks
            NPCID.Sets.NPCBestiaryDrawModifiers value = new()
            {
                Velocity = 1f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
        }

        public override void SetDefaults()
        {
            NPC.width = 34;
            NPC.height = 48;

            NPC.damage = 30;
            NPC.defense = 12;
            NPC.lifeMax = 180;

            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath6;

            NPC.value = 200f;
            NPC.knockBackResist = 0.2f;

            // ✔ REAL DARK CASTER AI
            NPC.aiStyle = NPCAIStyleID.Caster;
            AIType = NPCID.DarkCaster;
            AnimationType = NPCID.DarkCaster;

            // Banner
            Banner = NPC.type;
            BannerItem = ModContent.ItemType<SpiritReaperBanner>();
        }

        // --------------------------
        //      CUSTOM ATTACK
        // --------------------------
        public override void AI()
        {
            attackTimer++;

            if (attackTimer >= 120 && Main.netMode != NetmodeID.MultiplayerClient)
            {
                attackTimer = 0;

                Player target = Main.player[NPC.target];
                Vector2 direction = (target.Center - NPC.Center).SafeNormalize(Vector2.UnitX) * 8f;

                Projectile.NewProjectile(
                    NPC.GetSource_FromAI(),
                    NPC.Center,
                    direction,
                    ModContent.ProjectileType<SpiritScytheHostile>(),
                    12,
                    1f
                );

                NPC.netUpdate = true;
            }

            // BLOCK VANILLA PROJECTILES
            NPC.localAI[0] = 0f;
            NPC.localAI[1] = 0f;
            NPC.ai[2] = 1f;
        }

        // --------------------------
        //      LOOT TABLE
        // --------------------------
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            // Copy Dark Caster drops
            var casterDrops = Main.ItemDropsDB.GetRulesForNPCID(NPCID.DarkCaster, false);
            foreach (var rule in casterDrops)
                npcLoot.Add(rule);

            // Your custom drops
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ReapersArm>(), 500, 1, 1));
        }

        // --------------------------
        //      SPAWN RULES
        // --------------------------
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (spawnInfo.Player.ZoneDungeon)
                return 0.08f;

            return 0f;
        }

        // --------------------------
        //      BESTIARY ENTRY
        // --------------------------
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[]
            {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheDungeon,
                new FlavorTextBestiaryInfoElement(
                    "A resiliant spirit that took control of a Dark Caster. It reaps wandering souls with spectral scythes."
                )
            });
        }
    }

    // --------------------------
    //      BANNER ITEM
    // --------------------------
    public class SpiritReaperBanner : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/General/Placeable/SpiritReaperBanner";

        public override void SetDefaults()
        {
            Item.DefaultToPlaceableTile(ModContent.TileType<EnemyBanner>(), (int)EnemyBanner.StyleID.SpiritReaper);
            Item.width = 10;
            Item.height = 24;
            Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(silver: 10));
        }
    }

    // --------------------------
    //      HOSTILE PROJECTILE
    // --------------------------
    public class SpiritScytheHostile : ModProjectile
    {
        public override string Texture => "NaturiumMod/Assets/Items/General/Projectiles/SpiritScytheProj";

        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 45;

            Projectile.hostile = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

            Projectile.timeLeft = 300;
            Projectile.light = 0.6f;
        }

        public override void AI()
        {
            Projectile.rotation += 0.25f;
            Projectile.velocity *= 0.99f;

            if (Main.rand.NextBool(3))
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Ghost);
        }
    }
}