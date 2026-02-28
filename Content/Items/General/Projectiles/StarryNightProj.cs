using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace NaturiumMod.Content.Items.General.Projectiles;

public class StarryNightProj : ModProjectile
{
    public override string Texture => "NaturiumMod/Assets/Items/General/Projectiles/StarryNightProj";

    public override void SetStaticDefaults()
    {
        ProjectileID.Sets.IsAWhip[Type] = true;
    }

    public override void SetDefaults()
    {
        Projectile.DefaultToWhip();
    }

    float Timer
    {
        get => Projectile.ai[0];
        set => Projectile.ai[0] = value;
    }

    float ChargeTime
    {
        get => Projectile.ai[1];
        set => Projectile.ai[1] = value;
    }

    public override bool PreAI()
    {
        Player owner = Main.player[Projectile.owner];

        if (!owner.channel || ChargeTime >= 30)
            return true;

        if (++ChargeTime % 12 == 0)
            Projectile.WhipSettings.Segments++;

        Projectile.WhipSettings.RangeMultiplier += 1f / 30f;

        owner.itemAnimation = owner.itemAnimationMax;
        owner.itemTime = owner.itemTimeMax;

        return false;
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        target.AddBuff(ModContent.BuffType<AntiGravDebuff>(), 30);
        Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
    }

    void DrawLine(List<Vector2> points)
    {
        Texture2D tex = TextureAssets.FishingLine.Value;
        Rectangle frame = tex.Frame();
        Vector2 origin = new(frame.Width / 2, 2);

        Vector2 pos = points[0];

        for (int i = 0; i < points.Count - 1; i++)
        {
            Vector2 diff = points[i + 1] - points[i];
            float rotation = diff.ToRotation() - MathHelper.PiOver2;
            Color color = Lighting.GetColor(points[i].ToTileCoordinates(), Color.White);
            Vector2 scale = new(1, (diff.Length() + 2) / frame.Height);

            Main.EntitySpriteDraw(tex, pos - Main.screenPosition, frame, color, rotation, origin, scale, SpriteEffects.None, 0);
            pos += diff;
        }
    }

    public override bool PreDraw(ref Color lightColor)
    {
        List<Vector2> points = new();
        Projectile.FillWhipControlPoints(Projectile, points);

        DrawLine(points);

        SpriteEffects flip = Projectile.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

        Texture2D tex = TextureAssets.Projectile[Type].Value;
        Vector2 pos = points[0];

        for (int i = 0; i < points.Count - 1; i++)
        {
            Rectangle frame = new(0, 0, 10, 26);
            Vector2 origin = new(5, 8);
            float scale = 1f;

            if (i == points.Count - 2)
            {
                frame.Y = 74;
                frame.Height = 18;

                Projectile.GetWhipSettings(Projectile, out float timeToFlyOut, out _, out _);
                float t = Timer / timeToFlyOut;
                scale = MathHelper.Lerp(0.5f, 1.5f,
                    Utils.GetLerpValue(0.1f, 0.7f, t, true) *
                    Utils.GetLerpValue(0.9f, 0.7f, t, true));
            }
            else if (i > 10)
            {
                frame.Y = 58;
                frame.Height = 16;
            }
            else if (i > 5)
            {
                frame.Y = 42;
                frame.Height = 16;
            }
            else if (i > 0)
            {
                frame.Y = 26;
                frame.Height = 16;
            }

            Vector2 diff = points[i + 1] - points[i];
            float rotation = diff.ToRotation() - MathHelper.PiOver2;
            Color color = Lighting.GetColor(points[i].ToTileCoordinates());

            Main.EntitySpriteDraw(tex, pos - Main.screenPosition, frame, color, rotation, origin, scale, flip, 0);
            pos += diff;
        }

        return false;
    }
}
