using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Projectiles;

public class LeodrakesYoyoProj : ModProjectile
{
    private int shootTimer;

    public override void SetStaticDefaults()
    {
        // The following sets are only applicable to yoyo that use aiStyle 99.

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

        Projectile.aiStyle = ProjAIStyleID.Yoyo; // Projectile's AI style. Yoyos use aiStyle 99 (ProjAIStyleID.Yoyo). A lot of yoyo code checks for this aiStyle to work properly.

        Projectile.friendly = true; // Does damage to enemies but not to friendly Town NPCs.
        Projectile.DamageType = DamageClass.MeleeNoSpeed; // Benefits from melee bonuses. MeleeNoSpeed means the item will not scale with attack speed.
        Projectile.penetrate = -1; // All vanilla yoyos have infinite penetration. The number of enemies the yoyo can hit before being pulled back in is based on YoyosLifeTimeMultiplier.
                                   // Projectile.scale = 1f; // The scale of the projectile. Most yoyos are 1f, but a few are larger. The Kraken is the largest at 1.2f
    }

    public override void AI()
    {
        base.AI();

        shootTimer++;

        // Has timer reached interval (20 ticks = 1/3 second at 60 FPS)
        if (shootTimer >= 20)
        {
            shootTimer = 0;

            float randomAngleRadians = (float)(Main.rand.NextDouble() * MathHelper.TwoPi);
            Vector2 shootDirectionVector = new((float)Math.Cos(randomAngleRadians), (float)Math.Sin(randomAngleRadians));
            float projectileSpeed = 7f; // Adjust if needed

            // Apply direction and speed to projectile's velocity
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootDirectionVector * projectileSpeed, ModContent.ProjectileType<LeodrakesManeProj>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
        }
    }
}
