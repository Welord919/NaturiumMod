using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.General.Projectiles;

public class LeodrakesYoyoProj : ModProjectile
{
    private int shootTimer;
    public override string Texture => "NaturiumMod/Assets/Items/General/Projectiles/LeodrakesYoyoProj";

    public override void SetStaticDefaults()
    {
        // Time in seconds yoyo stays out before returning.
        // Vanilla: (Min = 3 wood) - (Max = 16 chik) | (Default = -1 infinite)
        ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = 7f;

        // Maximum distance yoyo sleep away from the player.
        // Vanilla: (Min = 130 wood) - (Max = 400 terrarian) | (Default = 200)
        ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 300f;

        // Top speed of projectile.
        // Vanilla: (Min = 9 wood) - (Max = 17.5 terrarian) | (Default = 10)
        ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 13f;
    }

    public override void SetDefaults()
    {
        Projectile.Size = new(16, 16);

        Projectile.aiStyle = ProjAIStyleID.Yoyo;

        Projectile.friendly = true;
        Projectile.DamageType = DamageClass.MeleeNoSpeed;
        Projectile.penetrate = -1;
    }

    public override void AI()
    {
        base.AI();

        shootTimer++;

        if (shootTimer >= 20)
        {
            shootTimer = 0;

            float randomAngleRadians = (float)(Main.rand.NextDouble() * MathHelper.TwoPi);
            Vector2 shootDirectionVector = new((float)Math.Cos(randomAngleRadians), (float)Math.Sin(randomAngleRadians));
            float projectileSpeed = 7f;

            // Apply direction and speed to projectile's velocity
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootDirectionVector * projectileSpeed, ModContent.ProjectileType<LeodrakesManeProj>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
        }
    }
}
