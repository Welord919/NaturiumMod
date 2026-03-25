using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NaturiumMod.Content.Helpers;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Cards.LOB.Commons
{
    public class Masaki : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/Masaki";

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 10;
            Item.useTime = 10;
            Item.channel = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;
            Item.DamageType = ModContent.GetInstance<CardDamage>();
            Item.damage = 20;
            Item.knockBack = 4f;
            Item.shoot = ModContent.ProjectileType<MasakiChargeProj>();
            Item.shootSpeed = 0f;
            Item.rare = ItemRarityID.Green;
            Item.value = Item.buyPrice(silver: 50);
            Item.consumable = true;
            Item.maxStack = 999;

        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source,
            Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.ownedProjectileCounts[type] == 0)
            {
                Projectile.NewProjectile(source, player.Center, Vector2.Zero, type,
                    damage, knockback, player.whoAmI);
            }
            return false;
        }
    }
    public class MasakiChargeProj : ModProjectile
    {
        public override string Texture => "NaturiumMod/Assets/Items/General/Projectiles/Blank";

        private int chargeTime = 0;
        private int stage = 1;

        private bool playedStartSound = false;
        private bool playedStage2 = false;
        private bool playedStage3 = false;
        private bool playedStage4 = false;

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.timeLeft = 9999;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.friendly = false;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (!player.channel)
            {
                ReleaseSlash(player);
                Projectile.Kill();
                return;
            }

            Projectile.Center = player.Center;
            // Spawn sheathed sword ONLY once
            if (player.ownedProjectileCounts[ModContent.ProjectileType<MasakiSheathed>()] == 0)
            {
                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    player.Center,
                    Vector2.Zero,
                    ModContent.ProjectileType<MasakiSheathed>(),
                    0,
                    0f,
                    player.whoAmI,
                    0f // we can pass stage later if needed
                );
            }


            chargeTime++;

            // Start charging sound
            if (!playedStartSound)
            {
                SoundEngine.PlaySound(SoundID.Item4, player.Center);
                playedStartSound = true;
            }

            // Stage thresholds
            if (chargeTime >= 240)
            {
                stage = 4;
                if (!playedStage4)
                {
                    SoundEngine.PlaySound(SoundID.Item15, player.Center);
                    playedStage4 = true;
                }
            }
            else if (chargeTime >= 120)
            {
                stage = 3;
                if (!playedStage3)
                {
                    SoundEngine.PlaySound(SoundID.Item15, player.Center);
                    playedStage3 = true;
                }
            }
            else if (chargeTime >= 60)
            {
                stage = 2;
                if (!playedStage2)
                {
                    SoundEngine.PlaySound(SoundID.Item15, player.Center);
                    playedStage2 = true;
                }
            }
            else
            {
                stage = 1;
            }

            Lighting.AddLight(player.Center, 0.3f * stage, 0.3f * stage, 0.1f);
        }

        private void ReleaseSlash(Player player)
        {
            foreach (Projectile p in Main.projectile)
            {
                if (p.active && p.owner == player.whoAmI && p.type == ModContent.ProjectileType<MasakiSheathed>())
                    p.Kill();
            }

            float damageMult = stage switch
            {
                1 => 1f,
                2 => 1.5f,
                3 => 2f,
                4 => 3f,
                _ => 1f
            };

            float scaleMult = stage switch
            {
                1 => 1f,
                2 => 1.25f,
                3 => 1.5f,
                4 => 2f,
                _ => 1f
            };

            float volume = stage switch
            {
                1 => 0.9f,
                2 => 1.1f,
                3 => 1.3f,
                4 => 1.5f,
                _ => 1f
            };

            SoundEngine.PlaySound(
                SoundID.Item1 with { Volume = volume, PitchVariance = 0.1f },
                player.Center
            );

            // Spawn slash(es)
            if (stage < 4)
            {
                // Normal single slash
                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    player.Center,
                    Vector2.Zero,
                    ModContent.ProjectileType<MasakiBlade>(),
                    (int)(Projectile.damage * damageMult),
                    Projectile.knockBack * scaleMult,
                    player.whoAmI,
                    scaleMult
                );
            }
            else
            {
                // ⭐ TRIPLE SLASH for stage 4 ⭐
                float[] angles = { -0.25f, 0f, 0.25f }; // slight angle offsets

                foreach (float offset in angles)
                {
                    Projectile.NewProjectile(
                        Projectile.GetSource_FromThis(),
                        player.Center,
                        new Vector2(offset, 0f), // pass offset through ai[1]
                        ModContent.ProjectileType<MasakiBlade>(),
                        (int)(Projectile.damage * damageMult),
                        Projectile.knockBack * scaleMult,
                        player.whoAmI,
                        scaleMult,
                        offset // ai[1] = rotation offset
                    );
                }
            }
        }
    }


    public class MasakiSheathed : ModProjectile
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/MasakiSheathe"; // or a sheathed sprite

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 999; // constantly refreshed
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            Projectile.direction = player.direction;
            Projectile.spriteDirection = 1; // we won't flip, we rotate instead

            // Hilt on abdomen
            Vector2 offset = player.direction == 1
                ? new Vector2(-6f, 10f)   // facing right: slightly behind + down
                : new Vector2(6f, 10f);   // facing left: slightly behind + down

            Projectile.Center = player.Center + offset;

            // Texture points right by default.
            // We want the blade pointing BACKWARDS along the hip.
            // Facing right → point left (Pi radians), slight tilt.
            // Facing left → point right (0 radians), slight tilt.
            Projectile.rotation = player.direction == 1
                ? MathHelper.Pi - 0.3f   // facing right, blade back
                : 0.3f;                  // facing left, blade back
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;

            float hiltOffset = 10f; // distance from left edge to hilt pivot in your PNG
            Vector2 origin = new Vector2(hiltOffset, tex.Height / 2f);
            Vector2 drawPos = Projectile.Center - Main.screenPosition;

            // Choose which side to flip. If the hilt is upside down when facing RIGHT, set true.
            bool flipWhenFacingRight = true;

            // Determine whether we should flip horizontally for this projectile
            bool shouldFlip = flipWhenFacingRight ? (Projectile.direction == 1) : (Projectile.direction == -1);
            SpriteEffects fx = shouldFlip ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            // When flipped horizontally, mirror the origin X so the pivot stays at the hilt
            if (shouldFlip)
                origin.X = tex.Width - hiltOffset;

            // Rotation: add Pi when flipped so the sprite remains upright
            float rot = Projectile.rotation;
            if (shouldFlip)
                rot += MathHelper.Pi;

            Main.EntitySpriteDraw(
                tex,
                drawPos,
                null,
                lightColor,
                rot,
                origin,
                0.8f,
                fx,
                0
            );

            return false;
        }
    }
    public class MasakiBlade : ModProjectile
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/MasakiBlade";

        public override void SetDefaults()
        {
            Projectile.width = 120;
            Projectile.height = 120;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

            Projectile.timeLeft = 60;

            Projectile.DamageType = ModContent.GetInstance<CardDamage>();
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!player.active || player.dead)
            {
                Projectile.Kill();
                return;
            }

            float scale = Projectile.ai[0];
            float angleOffset = Projectile.ai[1];

            if (Projectile.ai[2] == 0)
                Projectile.ai[2] = 1;

            Projectile.ai[2]++;
            float swingTimer = Projectile.ai[2];

            int baseDuration = 16;
            float swingDuration = baseDuration / scale;

            if (swingTimer >= swingDuration)
            {
                Projectile.Kill();
                return;
            }

            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;

            Vector2 dir = (Main.MouseWorld - player.Center)
                .SafeNormalize(new Vector2(player.direction, 0f));

            float baseRot = dir.ToRotation();
            float progress = swingTimer / swingDuration;
            progress = MathHelper.Clamp(progress, 0f, 1f);

            Projectile.rotation = baseRot + angleOffset + MathHelper.Lerp(-1.2f, 1.2f, progress);
            Projectile.Center = player.Center;

            player.direction = Main.MouseWorld.X >= player.Center.X ? 1 : -1;
            player.itemRotation = Projectile.rotation * player.direction;

            Projectile.scale = scale;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float collisionPoint = 0f;

            float scale = Projectile.ai[0];
            float hitboxScale = scale * 0.9f;

            float reach = 115f * hitboxScale;
            float thickness = 20f * hitboxScale;

            Vector2 start = Projectile.Center;
            Vector2 end = Projectile.Center + Projectile.rotation.ToRotationVector2() * reach;

            return Collision.CheckAABBvLineCollision(
                targetHitbox.TopLeft(),
                targetHitbox.Size(),
                start,
                end,
                thickness,
                ref collisionPoint
            );
        }

        public override bool PreDraw(ref Color lightColor)
        {
            var texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;

            Vector2 origin = new Vector2(20f, texture.Height / 2f);
            Vector2 drawPos = Projectile.Center - Main.screenPosition;

            Main.EntitySpriteDraw(
                texture,
                drawPos,
                null,
                lightColor,
                Projectile.rotation,
                origin,
                Projectile.scale,
                SpriteEffects.None,
                0
            );

            return false;
        }
    }

}