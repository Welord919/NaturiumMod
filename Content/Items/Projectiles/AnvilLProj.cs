using Terraria;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Projectiles
{
    public class AnvilLProj : ModProjectile
    {
        public override void SetDefaults()
        {
            // Set projectile properties
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true; // Determines if it can damage players
            Projectile.penetrate = 1; // How many enemies it can hit before disappearing
            Projectile.timeLeft = 600; // Time in ticks (60 ticks = 1 second)
        }

        public override void AI()
        {
            // Custom behavior for your projectile
            // For example, make it fall downward:
            Projectile.velocity.Y += 0.3f; // Adjust the falling speed
        }
    }
}