using Terraria;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Projectiles;

public class BarkionsBarkProj : ModProjectile
{
    public override void SetDefaults()
    {
        Projectile.width = 20;
        Projectile.height = 20;
        Projectile.aiStyle = 1;

        Projectile.friendly = true;
        Projectile.hostile = false;
        Projectile.ArmorPenetration = 1;

        //Projectile.DamageType = DamageClass.Ranged;

        Projectile.penetrate = 1;
        Projectile.timeLeft = 100;

        Projectile.ignoreWater = true;
        Projectile.tileCollide = true;

        Projectile.extraUpdates = 1;
    }
}
