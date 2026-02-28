using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

public class ApophisProj : ModProjectile
{
    public override string Texture => "NaturiumMod/Assets/Items/General/Projectiles/ApophisProj";

    public override void SetDefaults()
    {
        Projectile.width = 14;
        Projectile.height = 14;
        Main.projFrames[Projectile.type] = 4;

        Projectile.aiStyle = 0;
        Projectile.friendly = true;
        Projectile.DamageType = DamageClass.Melee;

        Projectile.penetrate = 1;
        Projectile.timeLeft = 120;

        Projectile.ignoreWater = true;
        Projectile.tileCollide = false; // goes through walls

        Projectile.light = 0.5f;
        Projectile.extraUpdates = 1;

        Projectile.alpha = 100; // slight transparency
    }

    public override void AI()
    {
        Projectile.velocity *= 0.99f; // slows it by 5% each frame

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
