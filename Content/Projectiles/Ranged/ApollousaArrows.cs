using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Projectiles.Ranged
{
    public class GoddessArrowProj : ModProjectile
    {
        public override string Texture => "NaturiumMod/Assets/Projectiles/GoddessArrowProj";

        public override void SetDefaults()
        {
            Projectile.width = 12;
            Projectile.height = 12;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 4;
            Projectile.timeLeft = 60;
            Projectile.extraUpdates = 1;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
        }

        public override void AI()
        {
            // Ensure projectile always faces its velocity
            if (Projectile.velocity.Length() > 0.1f)
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            // Light
            Lighting.AddLight(Projectile.Center, 0.6f, 0.6f, 1.2f);

            // Holy trail
            if (Main.rand.NextBool(3))
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.HallowedTorch);
                d.noGravity = true;
                d.scale = 1.2f;
            }

            // Homing behavior
            float maxDetect = 350f;
            float homingSpeed = 0.25f;

            NPC target = null;
            float closest = maxDetect;

            foreach (NPC npc in Main.npc)
            {
                if (npc.CanBeChasedBy() && !npc.friendly)
                {
                    float dist = Vector2.Distance(npc.Center, Projectile.Center);
                    if (dist < closest)
                    {
                        closest = dist;
                        target = npc;
                    }
                }
            }

            if (target != null)
            {
                Vector2 desired = Projectile.DirectionTo(target.Center) * 12f;
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, desired, homingSpeed);
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.DefenseEffectiveness *= 0f; // ignore defense
            modifiers.FlatBonusDamage += 10;
        }
    }

    public class GoddessJudgementProj : ModProjectile
    {
        public override string Texture => "NaturiumMod/Assets/Projectiles/GoddessJudgementProj";

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 60;
            Projectile.timeLeft = 60;
            Projectile.extraUpdates = 2;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
        }

        public override void AI()
        {
            // Ensure projectile always faces its velocity
            if (Projectile.velocity.Length() > 0.1f)
                Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;

            // Radiant light
            Lighting.AddLight(Projectile.Center, 1.4f, 1.1f, 0.6f);

            // Holy beam trail
            for (int i = 0; i < 2; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.GoldFlame);
                d.noGravity = true;
                d.scale = 1.4f;
            }

            // Slight acceleration
            Projectile.velocity *= 1.02f;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            target.AddBuff(BuffID.Daybreak, 300);
            modifiers.DefenseEffectiveness *= 0f;
            modifiers.FlatBonusDamage += 20;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            for (int i = 0; i < 20; i++)
            {
                Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.GoldFlame);
                d.noGravity = true;
                d.scale = 1.6f;
            }
            return true;
        }

    }
    public class GoddessBarrageController : ModProjectile
    {
        public override string Texture => "NaturiumMod/Assets/Projectiles/GoddessArrowProj";

        private int timer;
        private int shotsFired;

        public override void SetDefaults()
        {
            Projectile.width = 2;
            Projectile.height = 2;
            Projectile.friendly = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 60; // lasts long enough for 3 shots
            Projectile.tileCollide = false;
            Projectile.hide = true;
        }

        public override void AI()
        {
            timer++;

            // Fire every 9 ticks (~0.15 seconds)
            if (timer >= 9)
            {
                timer = 0;
                shotsFired++;

                Player player = Main.player[Projectile.owner];

                Vector2 velocity = Projectile.velocity;

                // Fire a Goddess Arrow
                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    player.Center,
                    velocity,
                    ModContent.ProjectileType<GoddessArrowProj>(),
                    (int)(Projectile.damage * 0.5f),
                    2f,
                    player.whoAmI
                );

                if (shotsFired >= 3)
                    Projectile.Kill();
            }
        }
    }
}
