using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModdingGang.Content.Items.Projectiles
{
    public class NaturiumClumpProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0; // The recording mode
        }

        public override void SetDefaults()
        {
            Projectile.width = 8; // The width of projectile hitbox
            Projectile.height = 8; // The height of projectile hitbox
            Projectile.aiStyle = 1; // The ai style of the projectile, please reference the source code of Terraria
            Projectile.friendly = true; // Can the projectile deal damage to enemies?
            Projectile.hostile = false; // Can the projectile deal damage to the player?
            Projectile.DamageType = DamageClass.Ranged; // Is the projectile shoot by a ranged weapon?
            Projectile.penetrate = 2; // How many monsters the projectile can penetrate. (OnTileCollide below also decrements penetrate for bounces as well)
            Projectile.timeLeft = 300; // The live time for the projectile (60 = 1 second, so 600 is 10 seconds)
            Projectile.light = 0.1f; // How much light emit around the projectile
            Projectile.ignoreWater = true; // Does the projectile's speed be influenced by water?
            Projectile.tileCollide = true; // Can the projectile collide with tiles?
            Projectile.extraUpdates = 1; // Set to above 0 if you want the projectile to update multiple time in a frame
        }

        public override void AI()
        {
            float maxDetectRadius = 400f; // The maximum radius at which the projectile can detect a target
            float projSpeed = 10f; // The speed at which the projectile moves

            NPC closestNPC = FindClosestNPC(maxDetectRadius);
            if (closestNPC == null)
            {
                return;
            }

            // Calculate the direction to the target
            Vector2 direction = closestNPC.Center - Projectile.Center;
            direction.Normalize();
            direction *= projSpeed;

            // Adjust the projectile's velocity to home in on the target
            Projectile.velocity = (Projectile.velocity * 19f + direction) / 20f;
        }

        private NPC FindClosestNPC(float maxDetectDistance)
        {
            NPC closestNPC = null;
            float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;

            foreach (NPC npc in Main.npc)
            {
                if (!npc.CanBeChasedBy(this))
                {
                    continue;
                }

                float sqrDistanceToNPC = Vector2.DistanceSquared(npc.Center, Projectile.Center);

                if (sqrDistanceToNPC < sqrMaxDetectDistance)
                {
                    sqrMaxDetectDistance = sqrDistanceToNPC;
                    closestNPC = npc;
                }
            }

            return closestNPC;
        }
    }
}