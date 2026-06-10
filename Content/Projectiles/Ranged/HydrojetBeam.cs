using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Projectiles.Ranged
{
    public class HydrojetBeam : ModProjectile
    {
        public override string Texture => "NaturiumMod/Assets/Projectiles/WaterBullet";

        public override void SetDefaults()
        {
            Projectile.width = 6;
            Projectile.height = 6;
            Projectile.friendly = true;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 200;
            Projectile.extraUpdates = 3; // Hitscan feel
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);

            Dust d = Dust.NewDustDirect(Projectile.position, 2, 2, DustID.Water);
            d.scale = 1.2f;
            d.noGravity = true;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Wet, 180);
            target.AddBuff(BuffID.Slow, 120);
        }
    }

}
