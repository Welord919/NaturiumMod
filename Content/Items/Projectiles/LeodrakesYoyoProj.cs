using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Projectiles;

public class LeodrakesYoyoProj : ModProjectile
{
    private int shootTimer;
    private Vector2 shootDirection;

    public override void SetStaticDefaults()
    {
        // The following sets are only applicable to yoyo that use aiStyle 99.

        // YoyosLifeTimeMultiplier is how long in seconds the yoyo will stay out before automatically returning to the player. 
        // Vanilla values range from 3f (Wood) to 16f (Chik), and defaults to -1f. Leaving as -1 will make the time infinite.
        ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = 7f;

        // YoyosMaximumRange is the maximum distance the yoyo sleep away from the player. 
        // Vanilla values range from 130f (Wood) to 400f (Terrarian), and defaults to 200f.
        ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 300f;

        // YoyosTopSpeed is top speed of the yoyo Projectile.
        // Vanilla values range from 9f (Wood) to 17.5f (Terrarian), and defaults to 10f.
        ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 13f;
    }

    public override void SetDefaults()
    {
        Projectile.width = 16; // The width of the projectile's hitbox.
        Projectile.height = 16; // The height of the projectile's hitbox.

        Projectile.aiStyle = ProjAIStyleID.Yoyo; // The projectile's ai style. Yoyos use aiStyle 99 (ProjAIStyleID.Yoyo). A lot of yoyo code checks for this aiStyle to work properly.

        Projectile.friendly = true; // Player shot projectile. Does damage to enemies but not to friendly Town NPCs.
        Projectile.DamageType = DamageClass.MeleeNoSpeed; // Benefits from melee bonuses. MeleeNoSpeed means the item will not scale with attack speed.
        Projectile.penetrate = -1; // All vanilla yoyos have infinite penetration. The number of enemies the yoyo can hit before being pulled back in is based on YoyosLifeTimeMultiplier.
                                   // Projectile.scale = 1f; // The scale of the projectile. Most yoyos are 1f, but a few are larger. The Kraken is the largest at 1.2f
    }

    public override void AI()
    {
        // Call the base AI method for yoyo behavior
        base.AI();

        // Increment the timer
        shootTimer++;

        // Check if the timer has reached the interval (20 ticks = 1/3 second at 60 FPS)
        if (shootTimer >= 20)
        {
            // Reset the timer
            shootTimer = 0;

            // Generate a random angle in radians
            float randomAngle = (float)(Main.rand.NextDouble() * MathHelper.TwoPi);

            // Convert the angle to a direction vector
            Vector2 shootDirection = new Vector2((float)Math.Cos(randomAngle), (float)Math.Sin(randomAngle));

            // Set the speed of the projectile
            float projectileSpeed = 7f; // Adjust this value as needed

            // Apply the direction and speed to the projectile's velocity
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, shootDirection * projectileSpeed, ModContent.ProjectileType<LeodrakesManeProj>(), Projectile.damage, Projectile.knockBack, Projectile.owner);
        }
    }
}
