using Microsoft.Xna.Framework;
using Terraria;
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

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Poisoned, 180);
            target.AddBuff(BuffID.Venom, 120);

            // 10% serpent bite
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
}
