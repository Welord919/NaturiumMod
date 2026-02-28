using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

public class ApophisProj : ModProjectile
{
    public override string Texture => "NaturiumMod/Assets/Items/General/Projectiles/ApophisProj";

    public override void SetDefaults()
    {
        // Match Enchanted Sword beam size
        Projectile.width = 14;
        Projectile.height = 14;

        // Match Enchanted Sword AI
        Projectile.aiStyle = 27;
        AIType = ProjectileID.EnchantedBeam;

        Projectile.friendly = true;
        Projectile.hostile = false;
        Projectile.DamageType = DamageClass.Melee; // Enchanted Sword beam is melee

        Projectile.penetrate = 1;       // same as Enchanted Beam
        Projectile.timeLeft = 60;       // same lifetime
        Projectile.ignoreWater = true;
        Projectile.tileCollide = true;

        Projectile.light = 0.5f;        // Enchanted Sword glow
        Projectile.extraUpdates = 1;    // smooth movement like the beam
    }

    public override void AI()
    {
        Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Shadowflame);
        d.noGravity = true;
        d.scale = 0.5f;

    }

}
