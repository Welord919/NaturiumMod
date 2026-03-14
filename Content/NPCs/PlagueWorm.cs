using Microsoft.Xna.Framework;
using NaturiumMod.Content.Items.General.Placeable;
using NaturiumMod.Content.Items.PreHardmode.Materials;
using NaturiumMod.Content.Items.Weapons.Melee;
using System.IO;
using Terraria;
using Terraria.Enums;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.NPCs
{
    // These three class showcase usage of the WormHead, WormBody and WormTail classes from Worm.cs
    
    internal class PlagueWormHead : WormHead
    {
        public override string Texture => "NaturiumMod/Assets/NPCs/ExampleWormHead";
        public override int BodyType => ModContent.NPCType<PlagueWormBody>();

        public override int TailType => ModContent.NPCType<PlagueWormTail>();

        public override void SetStaticDefaults()
        {
            var drawModifier = new NPCID.Sets.NPCBestiaryDrawModifiers()
            { // Influences how the NPC looks in the Bestiary
                CustomTexturePath = "NaturiumMod/Assets/NPCs/ExampleWorm_Bestiary", // If the NPC is multiple parts like a worm, a custom texture for the Bestiary is encouraged.
                Position = new Vector2(40f, 24f),
                PortraitPositionXOverride = 0f,
                PortraitPositionYOverride = 12f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawModifier);
        }

        public override void SetDefaults()
        {
            // Head is 10 defense, body 20, tail 30.
            NPC.CloneDefaults(NPCID.DiggerHead);
            NPC.aiStyle = -1;

            Banner = Type;
            // These lines are only needed in the main body part.
            BannerItem = ModContent.ItemType<PlagueWormHeadBanner>();
            ItemID.Sets.KillsToBanner[BannerItem] = 25; // Custom kill count required for banner drop and bestiary unlock. Omit this line for the default 50 kill count.
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            bool inDesert = spawnInfo.Player.ZoneDesert;
            bool inCorrupt = spawnInfo.Player.ZoneCorrupt;
            bool inCrimson = spawnInfo.Player.ZoneCrimson;

            if (inDesert && (inCorrupt || inCrimson))
                return 0.01f;

            return 0f;
        }
        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            var zombieDrops = Main.ItemDropsDB.GetRulesForNPCID(NPCID.DiggerHead, false);
            foreach (var rule in zombieDrops)
                npcLoot.Add(rule);

            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<PlagueChunk>(), 1, 4, 9));
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange([
				// Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.CorruptDesert,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.CorruptUndergroundDesert,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("A Worm that has grown to survive in the plague by infusing it into its body")
            ]);
        }
        public class PlagueWormHeadBanner : ModItem
        {
            public override string Texture => "NaturiumMod/Assets/Items/General/Placeable/PlagueWormHeadBanner";
            public override void SetDefaults()
            {
                Item.DefaultToPlaceableTile(ModContent.TileType<EnemyBanner>(), (int)EnemyBanner.StyleID.PlagueWormHead);
                Item.width = 10;
                Item.height = 24;
                Item.SetShopValues(ItemRarityColor.Blue1, Item.buyPrice(silver: 10));
            }
        }
        public class PlagueBlobProj : ModProjectile
        {
            public override string Texture => "NaturiumMod/Assets/Items/General/Projectiles/PlagueBlobProj";

            public override void SetDefaults()
            {
                Projectile.Size = new(20, 20);
                Projectile.aiStyle = ProjAIStyleID.Arrow;

                Projectile.friendly = false;
                Projectile.hostile = true;
                Projectile.ArmorPenetration = 2;

                Projectile.penetrate = 1;
                Projectile.timeLeft = 200;

                Projectile.ignoreWater = true;
                Projectile.tileCollide = true;

                Projectile.extraUpdates = 1;
            }
        }
        public override void Init()
        {
            // Set the segment variance
            // If you want the segment length to be constant, set these two properties to the same value
            MinSegmentLength = 6;
            MaxSegmentLength = 12;

            CommonWormInit(this);
        }

        // This method is invoked from ExampleWormHead, ExampleWormBody and ExampleWormTail
        internal static void CommonWormInit(Worm worm)
        {
            // These two properties handle the movement of the worm
            worm.MoveSpeed = 5.5f;
            worm.Acceleration = 0.045f;
        }

        private int attackCounter;
        public override void SendExtraAI(BinaryWriter writer)
        {
            writer.Write(attackCounter);
        }

        public override void ReceiveExtraAI(BinaryReader reader)
        {
            attackCounter = reader.ReadInt32();
        }

        public override void AI()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (attackCounter > 0)
                {
                    attackCounter--; // tick down the attack counter.
                }

                Player target = Main.player[NPC.target];
                // If the attack counter is 0, this NPC is less than 12.5 tiles away from its target, and has a path to the target unobstructed by blocks, summon a projectile.
                if (attackCounter <= 0 && Vector2.Distance(NPC.Center, target.Center) < 200 && Collision.CanHit(NPC.Center, 1, 1, target.Center, 1, 1))
                {
                    Vector2 direction = (target.Center - NPC.Center).SafeNormalize(Vector2.UnitX);
                    direction = direction.RotatedByRandom(MathHelper.ToRadians(10));

                    int projectile = Projectile.NewProjectile(NPC.GetSource_FromThis(), NPC.Top, direction * 6, ModContent.ProjectileType<PlagueBlobProj>(), 5, 0, Main.myPlayer);
                    Main.projectile[projectile].timeLeft = 300;
                    attackCounter = 500;
                    NPC.netUpdate = true;
                }
            }
        }
    }

    internal class PlagueWormBody : WormBody
    {
        public override string Texture => "NaturiumMod/Assets/NPCs/ExampleWormBody";
        public override void SetStaticDefaults()
        {
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
            NPCID.Sets.RespawnEnemyID[Type] = ModContent.NPCType<PlagueWormHead>();
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.DiggerBody);
            NPC.aiStyle = -1;

            // Extra body parts should use the same Banner value as the main ModNPC.
            Banner = ModContent.NPCType<PlagueWormHead>();
        }

        public override void Init()
        {
            PlagueWormHead.CommonWormInit(this);
        }
    }

    internal class PlagueWormTail : WormTail
    {
        public override string Texture => "NaturiumMod/Assets/NPCs/ExampleWormTail";
        public override void SetStaticDefaults()
        {
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, value);
            NPCID.Sets.RespawnEnemyID[Type] = ModContent.NPCType<PlagueWormHead>();
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.DiggerTail);
            NPC.aiStyle = -1;

            // Extra body parts should use the same Banner value as the main ModNPC.
            Banner = ModContent.NPCType<PlagueWormHead>();
        }

        public override void Init()
        {
            PlagueWormHead.CommonWormInit(this);
        }
    }
}