using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace NaturiumMod.Content.Items.General.Projectiles;

public class ExteriosWhipProj : ModProjectile
{
    public override string Texture => "NaturiumMod/Assets/Items/General/Projectiles/ExteriosWhipProj";

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

        if (!owner.channel || ChargeTime >= 17)
        {
            return true;
        }

        if (++ChargeTime % 12 == 0)
        {
            Projectile.WhipSettings.Segments++;
        }

        Projectile.WhipSettings.RangeMultiplier += 0.1f;

        owner.itemAnimation = owner.itemAnimationMax;
        owner.itemTime = owner.itemTimeMax;

        return false;
    }

    public void OnHitNPC(NPC target)
    {
        Main.player[Projectile.owner].MinionAttackTargetNPC = target.whoAmI;
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
        List<Vector2> list = [];
        Projectile.FillWhipControlPoints(Projectile, list);

        DrawLine(list);
        SpriteEffects flip = Projectile.spriteDirection < 0 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

        Main.instance.LoadProjectile(Type);
        Texture2D texture = TextureAssets.Projectile[Type].Value;

        Vector2 pos = list[0];

        for (int i = 0; i < list.Count - 1; i++)
        {
            Rectangle frame = new(0, 0, 10, 26);
            Vector2 origin = new(5, 8);
            float scale = 1;

            if (i == list.Count - 2)
            {
                frame.Y = 74;
                frame.Height = 18;

                Projectile.GetWhipSettings(Projectile, out float timeToFlyOut, out int _, out float _);
                float t = Timer / timeToFlyOut;
                scale = MathHelper.Lerp(0.5f, 1.5f, Utils.GetLerpValue(0.1f, 0.7f, t, true) * Utils.GetLerpValue(0.9f, 0.7f, t, true));
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

            Vector2 element = list[i];
            Vector2 diff = list[i + 1] - element;

            float rotation = diff.ToRotation() - MathHelper.PiOver2;
            Color color = Lighting.GetColor(element.ToTileCoordinates());

            Main.EntitySpriteDraw(texture, pos - Main.screenPosition, frame, color, rotation, origin, scale, flip, 0);

            pos += diff;
        }

        return false;
    }
}
