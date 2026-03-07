using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

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

        SpriteEffects flip = Projectile.spriteDirection < 0
            ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

        Texture2D texture = TextureAssets.Projectile[Type].Value;

        const int frameWidth = 4;
        const int frameHeight = 23;
        const int frameCount = 4;

        Vector2 pos = points[0];

        for (int i = 0; i < points.Count - 1; i++)
        {
            // Always use one of the 4 frames
            int frameIndex = Math.Min(i, frameCount - 1);

            Rectangle frame = new Rectangle(
                0,
                frameIndex * frameHeight,
                frameWidth,
                frameHeight
            );

            Vector2 origin = new Vector2(frameWidth / 2f, frameHeight / 2f);

            Vector2 element = points[i];
            Vector2 diff = points[i + 1] - element;

            float rotation = diff.ToRotation() - MathHelper.PiOver2;
            Color color = Lighting.GetColor(element.ToTileCoordinates());

            Main.EntitySpriteDraw(
                texture,
                pos - Main.screenPosition,
                frame,
                color,
                rotation,
                origin,
                1f,
                flip,
                0
            );

            pos += diff;
        }

        return false;
    }
}
