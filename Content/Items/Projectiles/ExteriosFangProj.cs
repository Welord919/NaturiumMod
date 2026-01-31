using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Projectiles;

public class ExteriosFangProj : ModProjectile
{
    public override void SetDefaults()
    {
        Projectile.Size = new(24, 24);
        Projectile.aiStyle = ProjAIStyleID.Arrow;
        Projectile.light = 2.0f;

        Projectile.friendly = true;
        Projectile.hostile = false;

        //Projectile.DamageType = DamageClass.Ranged;

        Projectile.penetrate = 5;
        Projectile.timeLeft = 300;

        Projectile.ignoreWater = true;
        Projectile.tileCollide = true;

        Projectile.extraUpdates = 1;
    }
}
