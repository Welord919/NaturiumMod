using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.General.Projectiles;

public class RoseWhipProj : ModProjectile
{
    public override string Texture => "NaturiumMod/Assets/Items/General/Projectiles/RoseWhipProj2";

    public override void SetStaticDefaults()
    {
        // This makes the projectile use whip collision detection and allows flasks to be applied to it.
        ProjectileID.Sets.IsAWhip[Type] = true;
    }

    public override void SetDefaults()
    {
        Projectile.DefaultToWhip();
    }

    private float Timer
    {
        get => Projectile.ai[0];
        set => Projectile.ai[0] = value;
    }

    private float ChargeTime
    {
        get => Projectile.ai[1];
        set => Projectile.ai[1] = value;
    }

    public override bool PreAI()
    {
        Player owner = Main.player[Projectile.owner];

        if (!owner.channel || ChargeTime >= 30)
        {
            return true;
        }

        if (++ChargeTime % 12 == 0)
        {
            Projectile.WhipSettings.Segments++;
        }

        Projectile.WhipSettings.RangeMultiplier += 1 / 30f;


        owner.itemAnimation = owner.itemAnimationMax;
        owner.itemTime = owner.itemTimeMax;

        return false;
    }

    public void OnHitNPC(NPC target)
    {
        Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        target.AddBuff(BuffID.Poisoned, 300);
    }

    private void DrawLine(List<Vector2> list)
    {
        Texture2D texture = TextureAssets.FishingLine.Value;
        Rectangle frame = texture.Frame();
        Vector2 origin = new(frame.Width / 2, 2);

        Vector2 pos = list[0];
        for (int i = 0; i < list.Count - 1; i++)
        {
            Vector2 element = list[i];
            Vector2 diff = list[i + 1] - element;

            float rotation = diff.ToRotation() - MathHelper.PiOver2;
            Color color = Lighting.GetColor(element.ToTileCoordinates(), Color.White);
            Vector2 scale = new(1, (diff.Length() + 2) / frame.Height);

            Main.EntitySpriteDraw(texture, pos - Main.screenPosition, frame, color, rotation, origin, scale, SpriteEffects.None, 0);

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
