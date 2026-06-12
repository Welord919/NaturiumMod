using Microsoft.Xna.Framework;
using NaturiumMod.Content.Items.Accessories.CraftingTrees;
using NaturiumMod.Content.Projectiles.Melee;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using static NaturiumMod.Content.Projectiles.Melee.ApophisProj;

namespace NaturiumMod.Content.Projectiles.Summoner
{
    public class ApophisEyeBuff : ModBuff
    {
        public override string Texture => "NaturiumMod/Assets/BuffsDebuffs/CosmoMinionBuff";

        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            // Keep buff alive only if Apophis Eye Minion exists
            if (player.ownedProjectileCounts[ModContent.ProjectileType<ApophisEyeMinion>()] > 0)
            {
                player.buffTime[buffIndex] = 18000;
            }
            else
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }
    public class AnubisJudgeBuff : ModBuff
    {
        public override string Texture => "NaturiumMod/Assets/BuffsDebuffs/CosmoMinionBuff";

        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            // Keep buff alive only if Anubis Obelisk Sentry exists
            if (player.ownedProjectileCounts[ModContent.ProjectileType<ApophisObeliskSentry>()] > 0)
            {
                player.buffTime[buffIndex] = 18000;
            }
            else
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }
    }

    public class ApophisEyeMinion : ModProjectile
    {
        public override string Texture => "NaturiumMod/Assets/Items/Millennium/ApophisEye";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionSacrificable[Type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 22;

            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.minionSlots = 1f;

            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 18000;

            Projectile.DamageType = DamageClass.Summon;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            // If the player lost the buff, remove the minion
            if (!player.HasBuff(ModContent.BuffType<ApophisEyeBuff>()))
            {
                Projectile.Kill();
                return;
            }

            // Keep alive
            Projectile.timeLeft = 2;

            // --- Orbit configuration ---
            const float baseOrbitRadius = 70f;      // base distance from player
            const float rotationSpeed = 0.01f;      // radians per tick (slow)
            const float smoothing = 0.35f;          // how quickly the eye moves to its target position (0-1)

            // Gather all active ApophisEyeMinion projectiles owned by this player
            List<int> owned = new List<int>();
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile p = Main.projectile[i];
                if (p.active && p.owner == Projectile.owner && p.type == Projectile.type)
                    owned.Add(p.whoAmI);
            }

            // Ensure stable ordering by sorting by whoAmI (projectile index)
            owned.Sort();

            // Find this projectile's index among the owned eyes
            int count = owned.Count;
            int myIndex = owned.IndexOf(Projectile.whoAmI);
            if (myIndex < 0) myIndex = 0; // fallback (shouldn't happen)

            // Compute step and global rotation
            float step = (count > 0) ? MathHelper.TwoPi / count : MathHelper.TwoPi;
            float globalRotation = Main.GameUpdateCount * rotationSpeed;

            // Final angle for this eye (equally spaced + global rotation)
            float angle = globalRotation + myIndex * step;

            // Smooth orbit radius (can be adjusted per-eye if desired)
            float orbitRadius = baseOrbitRadius;

            // Target position around the player
            Vector2 targetOffset = orbitRadius * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
            Vector2 targetPos = player.Center + targetOffset;

            // Smoothly move the projectile toward the target position to avoid snapping
            Projectile.Center = Vector2.Lerp(Projectile.Center, targetPos, smoothing);

            // Face direction (optional)
            Projectile.rotation = (targetPos - Projectile.Center).ToRotation();

            // --- Targeting and firing (unchanged behavior, just cleaned) ---
            NPC target = FindTarget(player, 550f);
            if (target != null)
                FireAtTarget(player, target);
        }

        private NPC FindTarget(Player player, float range)
        {
            NPC closest = null;
            float dist = range;

            foreach (NPC npc in Main.ActiveNPCs)
            {
                if (!npc.CanBeChasedBy()) continue;

                float d = Vector2.Distance(player.Center, npc.Center);
                if (d < dist)
                {
                    dist = d;
                    closest = npc;
                }
            }

            return closest;
        }

        private void FireAtTarget(Player player, NPC target)
        {
            if (Projectile.localAI[0] > 0)
            {
                Projectile.localAI[0]--;
                return;
            }

            // Faster fire rate when player is low on life
            Projectile.localAI[0] = player.statLife < player.statLifeMax2 / 2 ? 15 : 30;

            Vector2 direction = (target.Center - Projectile.Center).SafeNormalize(Vector2.UnitX) * 12f;

            Projectile.NewProjectile(
                Projectile.GetSource_FromThis(),
                Projectile.Center,
                direction,
                ModContent.ProjectileType<ApophisProjPlus>(),
                Projectile.damage,
                0f,
                player.whoAmI
            );
        }
    }
    public class ApophisObeliskSentry : ModProjectile
    {
        public override string Texture => "NaturiumMod/Assets/Projectiles/AnubisObelisk";

        public override void SetStaticDefaults()
        {
            Main.projFrames[Type] = 6;
        }

        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 74;

            Projectile.sentry = true;
            Projectile.friendly = true;
            Projectile.penetrate = -1;

            Projectile.timeLeft = Projectile.SentryLifeTime;
            Projectile.DamageType = DamageClass.Summon;

            Projectile.ignoreWater = true;
            Projectile.netImportant = true;
            Projectile.tileCollide = true;
        }

        public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
        {
            fallThrough = false;
            return true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.velocity = Vector2.Zero;
            return false;
        }

        private bool IsOnGround()
        {
            int tileX = (int)(Projectile.Center.X / 16f);
            int tileY = (int)((Projectile.Bottom.Y + 1f) / 16f);

            Tile tile = Framing.GetTileSafely(tileX, tileY);
            return tile.HasTile && Main.tileSolid[tile.TileType];
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (!player.HasBuff(ModContent.BuffType<AnubisJudgeBuff>()))
            {
                Projectile.Kill();
                return;
            }

            // -------------------------
            // GRAVITY (from AnubisSentry)
            // -------------------------
            Projectile.velocity.X = 0f;

            Projectile.velocity.Y += 0.4f;
            if (Projectile.velocity.Y > 10f)
                Projectile.velocity.Y = 10f;

            if (IsOnGround())
                Projectile.velocity.Y = 0f;

            // -------------------------
            // ORIGINAL OBLISK ATTACK LOGIC
            // -------------------------

            NPC target = FindTarget(Projectile.Center, 200f);

            if (target != null)
            {
                if (Projectile.localAI[2] != 1f)
                {
                    Projectile.localAI[2] = 1f;
                    Projectile.localAI[1] = 0f;
                    Projectile.frameCounter = 0;
                }

                Projectile.localAI[1]++;

                if (Projectile.localAI[1] < 30f)
                {
                    Projectile.frame = 2;
                }
                else if (Projectile.localAI[1] == 30f)
                {
                    Projectile.frame = 3;

                    SoundEngine.PlaySound(SoundID.Item74 with { Volume = 0.6f, Pitch = -0.2f }, Projectile.Center);

                    bool scarab = player.GetModPlayer<MillenniumScarabPlayer>().hasMillenniumScarab;

                    // Base radius
                    float baseRadius = 200f;

                    // Scarab increases hit radius by +20
                    float hitRadius = scarab ? baseRadius + 20f : baseRadius;

                    // Damage
                    int burstDamage = Projectile.damage;
                    if (scarab)
                        burstDamage = (int)(burstDamage * 1.25f);

                    // Apply damage + knockback
                    foreach (NPC npc in Main.ActiveNPCs)
                    {
                        if (!npc.CanBeChasedBy()) continue;

                        float dist = Vector2.Distance(npc.Center, Projectile.Center);

                        if (dist <= hitRadius)
                        {
                            Vector2 push = (npc.Center - Projectile.Center).SafeNormalize(Vector2.UnitY) * 12f;
                            npc.velocity += push;

                            NPC.HitInfo hit = new NPC.HitInfo
                            {
                                Damage = burstDamage,
                                Knockback = 0f,
                                HitDirection = npc.Center.X < Projectile.Center.X ? -1 : 1,
                                Crit = false
                            };
                            npc.StrikeNPC(hit);

                            if (Main.rand.NextBool(2))
                                npc.AddBuff(BuffID.Confused, 180);

                            npc.AddBuff(BuffID.Venom, 180);
                        }
                    }

                    // -------------------------
                    // BURST DUST (purple → gold if Scarab)
                    // -------------------------
                    int burstDust = scarab ? DustID.GoldCoin : DustID.PurpleTorch;
                    Color burstColor = scarab ? Color.Gold : Color.Purple;
                    float burstScale = scarab ? 1.6f : 1.5f;

                    for (int i = 0; i < 40; i++)
                    {
                        Vector2 vel = Main.rand.NextVector2Circular(6f, 6f);
                        Dust d = Dust.NewDustPerfect(Projectile.Center, burstDust, vel, 150, burstColor, burstScale);
                        d.noGravity = true;
                    }

                    // -------------------------
                    // RING DUST (accurate to hit radius)
                    // -------------------------

                    // Visual rings:
                    // Inner ring = hitRadius - 10  (210 when scarab)
                    // Offset ring = midpoint       (215 when scarab)
                    // Outer ring = hitRadius       (220 when scarab)

                    float innerRing = hitRadius - 10f;
                    float outerRing = hitRadius;
                    float offsetRing = (innerRing + outerRing) * 0.5f;

                    int ringDust = scarab ? DustID.GoldCoin : DustID.PurpleCrystalShard;
                    Color ringColor = scarab ? Color.Gold : Color.MediumPurple;
                    float ringScale = scarab ? 1.4f : 1.2f;

                    // INNER RING (210 when scarab)
                    for (int i = 0; i < 20; i++)
                    {
                        float angle = MathHelper.TwoPi * i / 20f;
                        Vector2 pos = Projectile.Center + Vector2.UnitX.RotatedBy(angle) * innerRing;

                        Dust d = Dust.NewDustPerfect(pos, ringDust, Vector2.Zero, 100, ringColor, ringScale);
                        d.noGravity = true;
                    }

                    // OFFSET RING (215 when scarab)
                    for (int i = 0; i < 20; i++)
                    {
                        float angle = MathHelper.TwoPi * (i + 0.5f) / 20f;
                        Vector2 pos = Projectile.Center + Vector2.UnitX.RotatedBy(angle) * offsetRing;

                        Dust d2 = Dust.NewDustPerfect(pos, ringDust, Vector2.Zero, 100, ringColor, ringScale * 0.9f);
                        d2.noGravity = true;
                    }

                    // OUTER RING (220 when scarab)
                    for (int i = 0; i < 20; i++)
                    {
                        float angle = MathHelper.TwoPi * i / 20f;
                        Vector2 pos = Projectile.Center + Vector2.UnitX.RotatedBy(angle) * outerRing;

                        Dust d3 = Dust.NewDustPerfect(pos, ringDust, Vector2.Zero, 100, ringColor, ringScale * 0.8f);
                        d3.noGravity = true;
                    }
                }
                else if (Projectile.localAI[1] > 30f && Projectile.localAI[1] < 60f)
                {
                    Projectile.frame = 3;
                }
                else if (Projectile.localAI[1] >= 60f)
                {
                    Projectile.localAI[1] = 0f;
                    Projectile.frame = 2;
                }
            }
            else
            {
                if (Projectile.localAI[2] != 0f)
                {
                    Projectile.localAI[2] = 0f;
                    Projectile.localAI[1] = 0f;
                    Projectile.frameCounter = 0;
                }

                if (++Projectile.frameCounter >= 60)
                    Projectile.frameCounter = 0;

                Projectile.frame = Projectile.frameCounter < 30 ? 0 : 1;
            }
        }


        private NPC FindTarget(Vector2 pos, float range)
        {
            NPC closest = null;
            float dist = range;

            foreach (NPC npc in Main.ActiveNPCs)
            {
                if (!npc.CanBeChasedBy()) continue;

                float d = Vector2.Distance(pos, npc.Center);
                if (d < dist)
                {
                    dist = d;
                    closest = npc;
                }
            }

            return closest;
        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 50; i++)
            {
                Vector2 speed = Main.rand.NextVector2CircularEdge(1f, 1f);
                Dust d = Dust.NewDustPerfect(Projectile.Center + speed * 50, DustID.BlueCrystalShard, speed * -5, Scale: 1.5f);
                d.noGravity = true;
            }
        }
    }


}
