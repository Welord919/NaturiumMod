
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.General.Projectiles;

public class SpiritScytheProj : ModProjectile
{
    public override string Texture => "NaturiumMod/Assets/Items/General/Projectiles/SpiritScytheProj";

    // how much lower the cursor grabs the scythe
    private const int MouseOffsetY = -24;

    public override void SetDefaults()
    {
        Projectile.width = 50;
        Projectile.height = 45;
        Projectile.friendly = true;
        Projectile.light = 0.8f;
        Projectile.DamageType = DamageClass.Magic;

        DrawOriginOffsetX = -25;
        DrawOriginOffsetY = -22;

        // infinite pierce + custom collision
        Projectile.penetrate = -1;
        Projectile.usesLocalNPCImmunity = true;
        Projectile.localNPCHitCooldown = 10;

        // channel weapon behavior
        Projectile.timeLeft = 2;
    }

    public override void OnSpawn(IEntitySource source)
    {
        SoundEngine.PlaySound(SoundID.Item9, Projectile.position);
    }

    public override Color? GetAlpha(Color lightColor) => new Color(255, 255, 255, 0);

    public override void AI()
    {
        Projectile.timeLeft = 2; // keeps projectile alive

        Player player = Main.player[Projectile.owner];
        player.AddBuff(ModContent.BuffType<NoManaRegenDebuff>(), 2);


        // stop if player stops channeling
        if (!player.channel)
        {
            Projectile.Kill();
            return;
        }

        // mana drain every 20 ticks (1 second)
        if (Projectile.ai[1]++ >= 20)
        {
            Projectile.ai[1] = 0;

            int manaCost = 3;

            if (player.statMana >= manaCost)
                player.statMana -= manaCost;
            else
            {
                Projectile.Kill();
                return;
            }
        }

        // compute bottom-left pivot once, lowered by MouseOffsetY
        Vector2 bottomLeft = Projectile.position + new Vector2(0, Projectile.height + MouseOffsetY);

        // SPINNING EFFECT
        Projectile.rotation += 0.25f;

        if (Main.myPlayer == Projectile.owner && Projectile.ai[0] == 0f)
        {
            if (player.channel)
            {
                float maxDistance = 18f;
                Vector2 vectorToCursor = Main.MouseWorld - bottomLeft;
                float distanceToCursor = vectorToCursor.Length();

                if (distanceToCursor > maxDistance)
                {
                    distanceToCursor = maxDistance / distanceToCursor;
                    vectorToCursor *= distanceToCursor;
                }

                Projectile.velocity = vectorToCursor;
            }
            else if (Projectile.ai[0] == 0f)
            {
                Projectile.netUpdate = true;

                float maxDistance = 14f;
                Vector2 vectorToCursor = Main.MouseWorld - bottomLeft;
                float distanceToCursor = vectorToCursor.Length();

                if (distanceToCursor == 0f)
                {
                    vectorToCursor = bottomLeft - player.Center;
                    distanceToCursor = vectorToCursor.Length();
                }

                distanceToCursor = maxDistance / distanceToCursor;
                vectorToCursor *= distanceToCursor;

                Projectile.velocity = vectorToCursor;

                if (Projectile.velocity == Vector2.Zero)
                    Projectile.Kill();

                Projectile.ai[0] = 1f;
            }
        }
    }

    // ROTATING LINE HITBOX
    public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
    {
        float bladeLength = 50f;
        float bladeThickness = 12f;

        // use the SAME pivot as movement + rotation
        Vector2 pivot = Projectile.position + new Vector2(0, Projectile.height + MouseOffsetY);

        // compute blade direction based on rotation
        Vector2 bladeStart = pivot;
        Vector2 bladeEnd = pivot + Projectile.rotation.ToRotationVector2() * bladeLength;

        float collisionPoint = 0f;

        return Collision.CheckAABBvLineCollision(
            targetHitbox.TopLeft(),
            targetHitbox.Size(),
            bladeStart,
            bladeEnd,
            bladeThickness,
            ref collisionPoint
        );
    }

    public override void OnKill(int timeLeft)
    {
        SoundEngine.PlaySound(SoundID.Item10, Projectile.position);
    }
}
