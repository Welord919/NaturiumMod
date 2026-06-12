using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NaturiumMod.Content.Items.Accessories.CraftingTrees;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Projectiles.Melee
{
    public class ApophisProj : ModProjectile
    {
        public override string Texture => "NaturiumMod/Assets/Projectiles/ApophisProj";

        public override void SetDefaults()
        {
            Projectile.width = 14;
            Projectile.height = 14;
            Main.projFrames[Projectile.type] = 4;

            Projectile.aiStyle = 0;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Generic;

            Projectile.penetrate = 1;
            Projectile.timeLeft = 120;

            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;

            Projectile.light = 0.5f;
            Projectile.extraUpdates = 1;

            Projectile.alpha = 50;
        }
        public override void OnSpawn(IEntitySource source)
        {
            Projectile.damage = Projectile.originalDamage;
        }

        public override void AI()
        {
            Projectile.velocity *= 0.99f;

            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 10)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);

            Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.PurpleTorch);
            d.noGravity = true;
            d.scale = 0.5f;
            d.velocity *= 0.2f;
        }
    }
    public class ApophisProjPlus : ModProjectile
    {
        public override string Texture => "NaturiumMod/Assets/Projectiles/ApophisLargeProj";
        private static readonly string GoldTexturePath = "NaturiumMod/Assets/Projectiles/ApophisLargeProjGold";

        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ModContent.ProjectileType<ApophisProj>());
            Projectile.penetrate = 3;
            Projectile.extraUpdates = 1;

            Main.projFrames[Projectile.type] = 8;

            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;

            Projectile.light = 0.5f;
            Projectile.extraUpdates = 1;

            Projectile.alpha = 50;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.damage = Projectile.originalDamage;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            Player player = Main.player[Projectile.owner];

            // Millennium Scarab synergy: +25% damage
            if (player.GetModPlayer<MillenniumScarabPlayer>().hasMillenniumScarab)
            {
                modifiers.SourceDamage *= 1.25f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            // Always apply these
            target.AddBuff(BuffID.Poisoned, 180);
            target.AddBuff(BuffID.Venom, 120);

            // If this projectile was spawned by the Twin Serpentine Blade (ai0 == 1f),
            // apply Shadowflame and the cursed flame spawn chance.
            if (Projectile.ai[0] == 1f)
            {
                // Apply Shadowflame (cursed flame)
                target.AddBuff(BuffID.ShadowFlame, 180);

                // 20% chance to spawn a cursed flame at the target
                if (Main.rand.NextFloat() < 0.20f)
                {
                    Projectile.NewProjectile(
                        Projectile.GetSource_FromThis(),
                        target.Center,
                        Vector2.Zero,
                        ProjectileID.CursedFlameFriendly,
                        Projectile.damage,
                        0f,
                        Projectile.owner
                    );
                }
            }

            // 10% serpent bite (venom fang) — keep this for all sources
            if (Main.rand.NextFloat() < 0.10f)
            {
                Vector2 biteVel = Main.rand.NextVector2Circular(4f, 4f);
                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    target.Center,
                    biteVel,
                    ProjectileID.VenomFang,
                    Projectile.damage / 2,
                    0f,
                    Projectile.owner
                );
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            bool scarab = player.GetModPlayer<MillenniumScarabPlayer>().hasMillenniumScarab;

            // Choose texture
            Texture2D texture = scarab
                ? ModContent.Request<Texture2D>(GoldTexturePath).Value
                : ModContent.Request<Texture2D>(Texture).Value;

            // Animation frame rectangle
            int frameHeight = texture.Height / Main.projFrames[Projectile.type];
            Rectangle frame = new Rectangle(0, frameHeight * Projectile.frame, texture.Width, frameHeight);

            // Origin
            Vector2 origin = new Vector2(texture.Width / 2f, frameHeight / 2f);

            // Draw
            Main.EntitySpriteDraw(
                texture,
                Projectile.Center - Main.screenPosition,
                frame,
                Color.White * Projectile.Opacity,
                Projectile.rotation,
                origin,
                Projectile.scale,
                SpriteEffects.None,
                0
            );

            return false; // we handled drawing
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            bool scarab = player.GetModPlayer<MillenniumScarabPlayer>().hasMillenniumScarab;

            Projectile.velocity *= 0.99f;

            // Animation
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 10)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }

            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);

            // -------------------------
            // TRAIL DUST (purple → gold)
            // -------------------------
            int dustType = scarab ? DustID.GoldCoin : DustID.PurpleTorch;
            Color dustColor = scarab ? Color.Gold : Color.Purple;
            float dustScale = scarab ? 0.7f : 0.5f;

            Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, dustType);
            d.noGravity = true;
            d.scale = dustScale;
            d.color = dustColor;
            d.velocity *= 0.2f;
        }
    }

}
