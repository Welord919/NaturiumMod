using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NaturiumMod.Content.Items.PreHardmode.Accessories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.NPCs
{
    public class BalloonLizard : ModNPC
    {
        public override string Texture => "NaturiumMod/Assets/NPCs/BalloonLizard";
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[NPC.type] = 1;
        }
        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry entry)
        {
            entry.Info.AddRange(new IBestiaryInfoElement[]
            {
        BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Desert,
        new FlavorTextBestiaryInfoElement("A balloon shaped like a lizard. Surprisingly aggressive.")
            });
        }

        public override void SetDefaults()
        {
            NPC.width = 32;
            NPC.height = 42;
            NPC.damage = 10;
            NPC.defense = 4;
            NPC.lifeMax = 40;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 50f;
            NPC.knockBackResist = 0.6f;

            NPC.noGravity = true;     // Floats
            NPC.noTileCollide = true; // Drifts through air
            NPC.aiStyle = -1;         // Custom AI
        }
        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            Vector2 origin = texture.Size() / 2f;

            spriteBatch.Draw(
                texture,
                NPC.Center - screenPos,
                null,
                drawColor,
                NPC.rotation,
                origin,
                0.2f, // SCALE HERE (500px → 100px)
                SpriteEffects.None,
                0f
            );

            return false; // we handled drawing
        }

        public override void DrawEffects(ref Color drawColor)
        {
            NPC.scale = 0.2f; // 20% of original size (500 → 100)
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            bool windy = Main.WindyEnoughForKiteDrops;
            bool desert = spawnInfo.Player.ZoneDesert;

            if (windy && desert)
                return 0.25f; // 25% of normal spawns

            return 0f;
        }

        public override void AI()
        {
            // Always target the closest player
            NPC.TargetClosest();

            Player target = Main.player[NPC.target];

            // Bobbing motion (side-to-side)
            NPC.velocity.X = 1.2f * (float)System.Math.Sin(NPC.ai[0] / 40f);

            // Slight upward float
            NPC.velocity.Y -= 0.02f; // gentle rise, not overpowering

            // Homing toward the player
            Vector2 toPlayer = target.Center - NPC.Center;
            float distance = toPlayer.Length();

            if (distance < 800f) // only home if player is reasonably close
            {
                toPlayer.Normalize();

                // Stronger homing force so it actually reaches the player
                NPC.velocity += toPlayer * 0.15f;
            }

            // Cap speed so it stays floaty, not aggressive
            float maxSpeed = 3f;
            if (NPC.velocity.Length() > maxSpeed)
                NPC.velocity = Vector2.Normalize(NPC.velocity) * maxSpeed;

            NPC.ai[0]++;
        }


        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.GreenFairy);
                }
            }
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<LizardScale>(), 1, 1, 3));
        }
    }

    public class LizardScale : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Materials/LizardScales";
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 20;
            Item.maxStack = 999;
            Item.value = Item.buyPrice(copper: 50);
            Item.rare = ItemRarityID.White;
        }
    }
}
