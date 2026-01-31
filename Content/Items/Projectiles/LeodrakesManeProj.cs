using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ID;

namespace NaturiumMod.Content.Items.Projectiles;

public class LeodrakesManeProj : ModProjectile
{
    public override void SetDefaults()
    {
        Projectile.Size = new(20, 20);
        Projectile.aiStyle = ProjAIStyleID.Arrow;

        Projectile.friendly = true;
        Projectile.hostile = false;
        //Projectile.ArmorPenetration = 1;

        Projectile.DamageType = DamageClass.Magic;

        Projectile.penetrate = 1;
        Projectile.timeLeft = 150;

        Projectile.ignoreWater = true;
        Projectile.tileCollide = true;

        Projectile.extraUpdates = 1;
    }

    public override void AI()
    {
        float maxDetectRadius = 400f;   // The maximum radius at which the projectile can detect a target
        float projSpeed = 10f;          // The speed at which the projectile moves
        float trackingStrength = 0.05f; // Adjust this value to control the tracking strength

        NPC closestNPC = FindClosestNPC(maxDetectRadius);
        if (closestNPC is null)
        {
            return;
        }

        // Calculate the direction to the target
        Vector2 direction = closestNPC.Center - Projectile.Center;
        direction.Normalize();
        direction *= projSpeed;

        // Adjust the projectile's velocity to home in on the target more gradually
        Projectile.velocity = Vector2.Lerp(Projectile.velocity, direction, trackingStrength);
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
