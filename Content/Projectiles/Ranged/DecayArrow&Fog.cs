using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NaturiumMod.Content.BuffsDebuffs;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Projectiles.Ranged
{
    public class DecayArrowProj : ModProjectile
    {
        public override string Texture => "NaturiumMod/Assets/Projectiles/DecayArrowProj";

        public override void SetDefaults()
        {
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.aiStyle = ProjAIStyleID.Arrow;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 600;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<DecayDebuff>(), 180);
        }

        public override void OnKill(int timeLeft)
        {
            // ALWAYS spawn fog (this is the version that worked)
            Projectile.NewProjectile(
                Projectile.GetSource_Death(),
                Projectile.Center,
                Vector2.Zero,
                ModContent.ProjectileType<DecayFog>(),
                0,
                0f,
                Projectile.owner
            );

            // Dust burst
            for (int i = 0; i < 20; i++)
            {
                int dust = Dust.NewDust(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.Poisoned,
                    Main.rand.NextFloat(-2, 2),
                    Main.rand.NextFloat(-2, 2),
                    150,
                    default,
                    1.4f
                );
                Main.dust[dust].noGravity = true;
            }
        }
    }

    public class DecayFog : ModProjectile
    {
        public override string Texture => "Terraria/Images/Gore_" + GoreID.Smoke2;

        private int damageTimer = 0;

        public override void SetDefaults()
        {
            Projectile.width = 80;
            Projectile.height = 80;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 240; // 4 seconds
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

            Projectile.alpha = 0; // start fully visible
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;

            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            Vector2 origin = texture.Size() / 2f;

            float scale = 2.2f; // scale up the fog

            // Purple tint
            Color fogColor = new Color(180, 0, 255) * (1f - Projectile.alpha / 255f);

            Main.EntitySpriteDraw(
                texture,
                drawPos,
                null,
                fogColor,
                Projectile.rotation,
                origin,
                scale,
                SpriteEffects.None,
                0
            );

            return false; // we handled drawing
        }

        public override void AI()
        {
            // Fade out over time (your old working fade)
            Projectile.alpha = (int)MathHelper.Lerp(0, 255, 1f - Projectile.timeLeft / 240f);

            // Fog dust
            if (Main.rand.NextBool(3))
            {
                int dust = Dust.NewDust(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.Poisoned,
                    Main.rand.NextFloat(-1, 1),
                    Main.rand.NextFloat(-1, 1),
                    150,
                    new Color(180, 0, 255),
                    1.2f
                );
                Main.dust[dust].noGravity = true;
            }

            // Damage timer for 5 DPS
            damageTimer++;

            foreach (NPC npc in Main.npc)
            {
                if (npc.active && !npc.friendly && npc.Distance(Projectile.Center) < 60f)
                {
                    // Apply debuff
                    npc.AddBuff(ModContent.BuffType<DecayDebuff>(), 60);

                    // 5 DPS = 1 damage every 12 ticks
                    if (damageTimer >= 12)
                    {
                        npc.StrikeNPC(new NPC.HitInfo()
                        {
                            Damage = 1,
                            Knockback = 0f,
                            HitDirection = 0,
                            Crit = false
                        });
                    }
                }
            }

            if (damageTimer >= 12)
                damageTimer = 0;
        }
    }
}
