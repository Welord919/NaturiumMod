using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

public class SalamandraFireProj : ModProjectile
{
    public override string Texture => "NaturiumMod/Assets/Projectiles/SalamandraFireProj";

    public override void SetDefaults()
    {
        Projectile.width = 150;
        Projectile.height = 150;

        Projectile.friendly = true;
        Projectile.hostile = false;
        Projectile.penetrate = 20;
        Projectile.timeLeft = 120;

        Projectile.tileCollide = false;
        Projectile.ignoreWater = true;

        Projectile.DamageType = DamageClass.Melee;
        Projectile.extraUpdates = 1;
    }

    public override void OnSpawn(IEntitySource source)
    {
        SoundEngine.PlaySound(SoundID.Item20 with { Pitch = 0.2f }, Projectile.Center);
    }

    public override void AI()
    {
        Projectile.velocity *= 0.99f;

        Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.ToRadians(90f);

        Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Lava);
        d.noGravity = true;
        d.scale = 0.5f;
        d.velocity *= 0.5f;
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        // Apply Hellfire
        target.AddBuff(BuffID.OnFire3, 180);

        // Small burst of flame on hit
        for (int i = 0; i < 12; i++)
        {
            Dust d = Dust.NewDustDirect(target.position, target.width, target.height, DustID.Lava);
            d.velocity = Main.rand.NextVector2Circular(4f, 4f);
            d.scale = 1.4f;
            d.noGravity = true;
        }
    }

    public override bool PreDraw(ref Color lightColor)
    {
        Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;

        Vector2 origin = new Vector2(tex.Width / 2f, tex.Height / 2f);

        Main.EntitySpriteDraw(
            tex,
            Projectile.Center - Main.screenPosition,
            null,
            Color.White,
            Projectile.rotation,
            origin,
            1f,
            SpriteEffects.None,
            0
        );

        return false;
    }
}
