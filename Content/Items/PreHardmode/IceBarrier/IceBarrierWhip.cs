using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.PreHardmode.IceBarrier
{
    public class IceBarrierWhip : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/IceBarrier/IceBarrierWhip";

        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 40;

            // Whip setup
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.noMelee = true;
            Item.noUseGraphic = true;
            Item.channel = false;

            // Hybrid damage
            Item.DamageType = DamageClass.MagicSummonHybrid;
            Item.damage = 20;
            Item.knockBack = 2f;

            // Magic scaling baked in
            Item.GetGlobalItem<IceBarrierWhipGlobal>().magicScaling = 0.20f;

            Item.shoot = ModContent.ProjectileType<IceBarrierWhipProj>();
            Item.shootSpeed = 1f;

            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(silver: 80);
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<IceBarrierCore>(), 15);
            recipe.AddIngredient(ItemID.FrostburnArrow, 25);
            recipe.AddIngredient(ItemID.Sapphire, 8);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }

    }

    // Adds magic scaling to a summon weapon
    public class IceBarrierWhipGlobal : GlobalItem
    {
        public float magicScaling;

        public override bool InstancePerEntity => true;

        public override void ModifyWeaponDamage(Item item, Player player, ref StatModifier damage)
        {
            if (magicScaling > 0f)
            {
                damage += player.GetDamage(DamageClass.Magic).Additive * magicScaling;
            }
        }
    }

    public class IceBarrierWhipProj : ModProjectile
    {
        public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/IceBarrier/IceBarrierWhipProj";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.IsAWhip[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.DefaultToWhip();

            Projectile.WhipSettings.Segments = 12;
            Projectile.WhipSettings.RangeMultiplier = 8.0f;

        }


        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Frostburn, 180);

            // Ice shard burst
            for (int i = 0; i < 4; i++)
            {
                Vector2 vel = Main.rand.NextVector2Circular(3f, 3f);
                Dust.NewDustPerfect(target.Center, DustID.IceTorch, vel, 150, Color.Cyan, 1.2f);
            }
        }

        private void DrawLine(List<Vector2> points)
        {
            Texture2D texture = TextureAssets.FishingLine.Value;
            Rectangle frame = texture.Frame();
            Vector2 origin = new(frame.Width / 2, 2);

            Vector2 pos = points[0];

            for (int i = 0; i < points.Count - 1; i++)
            {
                Vector2 element = points[i];
                Vector2 diff = points[i + 1] - element;

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

            const int frameWidth = 14;
            const int frameHeight = 31;
            const int frameCount = 4;


            Vector2 pos = points[0];

            for (int i = 0; i < points.Count - 1; i++)
            {
                int frameIndex = Math.Min(i, frameCount - 1);

                Rectangle frame = new Rectangle(
                    0,
                    frameIndex * frameHeight,
                    frameWidth,
                    frameHeight
                );

                Vector2 origin = new(frameWidth / 2f, frameHeight / 2f);

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

}