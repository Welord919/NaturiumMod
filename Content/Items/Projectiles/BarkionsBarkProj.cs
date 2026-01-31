using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Projectiles;

public class BarkionsBarkProj : ModProjectile
{
    public override string Texture => "NaturiumMod/Assets/Items/Projectiles/BarkionsBarkProj";

    public override void SetDefaults()
    {
        Projectile.Size = new(20, 20);
        Projectile.aiStyle = ProjAIStyleID.Arrow;

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
