using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Projectiles.Magic
{
    public class NibiruStarProj : ModProjectile
    {
        public override string Texture => "NaturiumMod/Assets/Projectiles/NibiruStarProj";

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = 2;
            Projectile.timeLeft = 600;
            Projectile.aiStyle = 0;
            Projectile.alpha = 0;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.localAI[1] = Main.rand.NextFloat(-0.6f, 0.6f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;
            int frameHeight = tex.Height / Main.projFrames[Projectile.type];
            Rectangle source = new Rectangle(0, frameHeight * Projectile.frame, tex.Width, frameHeight);
            Vector2 origin = new Vector2(tex.Width * 0.5f, frameHeight * 0.5f);
            SpriteEffects effects = Projectile.spriteDirection == -1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

            Main.spriteBatch.Draw(
                tex,
                Projectile.Center - Main.screenPosition,
                source,
                Projectile.GetAlpha(lightColor),
                Projectile.rotation,
                origin,
                Projectile.scale,
                effects,
                0f
            );

            return false;
        }

        public override void AI()
        {
            Projectile.rotation += Projectile.localAI[1];

            if (Main.rand.NextBool(3))
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.GemSapphire);
                d.scale = Main.rand.NextFloat(0.9f, 1.3f);
                d.velocity = new Vector2(Main.rand.NextFloat(-0.3f, 0.3f), Main.rand.NextFloat(0.2f, 0.8f));
                d.noGravity = true;
                d.fadeIn = 0.5f;
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            if (Main.myPlayer == Projectile.owner)
            {
                Projectile.NewProjectile(
                    Projectile.GetSource_OnHit(target),
                    Projectile.Center,
                    Vector2.Zero,
                    ModContent.ProjectileType<StarfieldAoE>(),
                    Projectile.damage / 2,
                    0f,
                    Projectile.owner
                );
            }
        }
    }

    public class StarfieldAoE : ModProjectile
    {
        public override string Texture => "NaturiumMod/Assets/Projectiles/StarfieldAoE";
        int tickTimer;

        public override void SetDefaults()
        {
            Main.projFrames[Projectile.type] = 4;
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 160;
            Projectile.tileCollide = false;
            Projectile.alpha = 0;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.frame = Main.rand.Next(Main.projFrames[Projectile.type]);
            Projectile.frameCounter = Main.rand.Next(0, 4);
        }

        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 4)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;
                if (Projectile.frame >= Main.projFrames[Projectile.type])
                    Projectile.frame = 0;
            }

            if (Main.rand.NextBool(6))
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GemSapphire);

            tickTimer++;
            if (tickTimer >= 12)
            {
                tickTimer = 0;
                if (Main.myPlayer == Projectile.owner)
                {
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero,
                        ModContent.ProjectileType<StarfieldTick>(), Projectile.damage, 0f, Projectile.owner);
                }
            }

            Projectile.localAI[0]++;
            int fadeDuration = 120;
            float progress = Projectile.localAI[0] / (float)fadeDuration;
            progress = MathHelper.Clamp(progress, 0f, 1f);
            Projectile.alpha = (int)(progress * 255f);

            if (progress >= 1f)
                Projectile.Kill();
        }

        public class StarfieldTick : ModProjectile
        {
            public override string Texture => "NaturiumMod/Assets/Projectiles/Blank";
            public override void SetDefaults()
            {
                Projectile.width = 16;
                Projectile.height = 16;
                Projectile.friendly = true;
                Projectile.hostile = false;
                Projectile.DamageType = DamageClass.Magic;
                Projectile.timeLeft = 2;
                Projectile.penetrate = -1;
                Projectile.tileCollide = false;
            }
        }
    }
}
