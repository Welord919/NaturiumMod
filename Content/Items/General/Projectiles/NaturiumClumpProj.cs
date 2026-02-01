using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.General.Projectiles;

public class NaturiumClumpProj : ModProjectile
{
    public override string Texture => "NaturiumMod/Assets/Items/General/Projectiles/NaturiumClumpProj";

    public override void SetStaticDefaults()
    {
        ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5; // The length of old position to be recorded
        ProjectileID.Sets.TrailingMode[Projectile.type] = 0; // The recording mode
    }

    public override void SetDefaults()
    {
        Projectile.Size = new(8, 8);
        Projectile.aiStyle = ProjAIStyleID.Arrow;
        Projectile.friendly = true; // Can the projectile deal damage to enemies?
        Projectile.hostile = false; // Can the projectile deal damage to the player?
        Projectile.DamageType = DamageClass.Ranged;
        Projectile.penetrate = 2; // How many monsters the projectile can penetrate. (OnTileCollide below also decrements penetrate for bounces as well)
        Projectile.timeLeft = 300; // Ticks (60 = 1 second, so 600 is 10 seconds)
        Projectile.light = 0.1f; // How much light emit around the projectile
        Projectile.ignoreWater = true; // Does the projectile's speed be influenced by water?
        Projectile.tileCollide = true; // Can the projectile collide with tiles?
        Projectile.extraUpdates = 1; // Set to above 0 if you want the projectile to update multiple time in a frame
    }

    public override void AI()
    {
        float maxTargetDetectRadius = 400f;
        float projectileMoveSpeed = 10f;

        NPC closestNPC = FindClosestNPC(maxTargetDetectRadius);
        if (closestNPC == null)
        {
            return;
        }

        Vector2 targetDirection = closestNPC.Center - Projectile.Center;
        targetDirection.Normalize();
        targetDirection *= projectileMoveSpeed;

        // Adjust the projectile's velocity to home in on the target
        Projectile.velocity = (Projectile.velocity * 19f + targetDirection) / 20f;
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
