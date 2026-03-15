using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.Cards;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Cards.LOB.SuperRares
{
    public class FlameSwordsman : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/FlameSwordsman";

        private int usesLeft = 5;
        private int swingIndex = 0; // 0 = top→bottom, 1 = bottom→top

        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 36;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 25;
            Item.useAnimation = 25;

            Item.noUseGraphic = true;
            Item.noMelee = true;

            Item.DamageType = ModContent.GetInstance<CardDamage>();
            Item.damage = 50;
            Item.knockBack = 6f;

            Item.rare = ItemRarityID.Orange;
            Item.value = Item.buyPrice(gold: 2);

            Item.UseSound = SoundID.Item20;


            Item.shoot = ModContent.ProjectileType<FlameSwordsmanSlash>();
            Item.shootSpeed = 0f;

            
            Item.maxStack = 999;

        }

        public override bool CanUseItem(Player player)
        {
            if (usesLeft <= 0)
                return false;

            if (player.HasBuff(ModContent.BuffType<SummoningSickness>()))
                return false;

            return true;
        }

        public override bool? UseItem(Player player)
        {
            usesLeft--;

            swingIndex ^= 1; // alternate swing direction

            player.AddBuff(ModContent.BuffType<SummoningSickness>(), 30);

            if (Main.myPlayer == player.whoAmI)
            {
                Vector2 spawnPos = player.Center;

                Vector2 dir = (Main.MouseWorld - player.Center)
                    .SafeNormalize(new Vector2(player.direction, 0f));

                int proj = Projectile.NewProjectile(
                    player.GetSource_ItemUse(Item),
                    spawnPos,
                    dir,
                    Item.shoot,
                    Item.damage,
                    Item.knockBack,
                    player.whoAmI,
                    swingIndex
                );

                if (proj >= 0)
                    Main.projectile[proj].originalDamage = Item.damage;
            }

            if (usesLeft <= 0)
            {
                Item.stack--;      // consume ONE card
                usesLeft = 5;      // reset charges for the next card in the stack
            }


            return true;
        }

        // ⭐ Prevent double‑spawn from swing useStyle
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source,
            Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return false;
        }
    }

    public class FlameSwordsmanSlash : ModProjectile
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/FlameSwordsmanSlash";

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
        }

        public override void SetDefaults()
        {
            Projectile.width = 120;
            Projectile.height = 120;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 16;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
            Projectile.spriteDirection = 1;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (!player.active || player.dead)
            {
                Projectile.Kill();
                return;
            }

            player.heldProj = Projectile.whoAmI;
            player.itemTime = 2;
            player.itemAnimation = 2;

            Vector2 baseDir = (Main.MouseWorld - player.Center)
                .SafeNormalize(new Vector2(player.direction, 0f));

            float baseRot = baseDir.ToRotation();
            float progress = 1f - Projectile.timeLeft / 16f;
            progress = MathHelper.Clamp(progress, 0f, 1f);

            bool bottomToTop = Projectile.ai[0] == 1f;
            float swingRange = MathHelper.PiOver2;

            float offset = bottomToTop
                ? MathHelper.Lerp(-swingRange, swingRange, progress)
                : MathHelper.Lerp(swingRange, -swingRange, progress);

            Projectile.rotation = baseRot + offset;
            Projectile.rotation %= MathHelper.TwoPi;

            Projectile.Center = player.Center;

            player.direction = Main.MouseWorld.X >= player.Center.X ? 1 : -1;
            player.itemRotation = Projectile.rotation * player.direction;

            Lighting.AddLight(Projectile.Center, 1f, 0.5f, 0.1f);

            Vector2 dir = Projectile.rotation.ToRotationVector2();
            float drawScale = 2f;
            float bladeLength = 120f * drawScale * 0.8f;
            int dustCount = 12;

            for (int i = 0; i < dustCount; i++)
            {
                float bladeProgress = i / (float)(dustCount - 1);
                Vector2 pos = Projectile.Center + dir * (bladeLength * bladeProgress);
                pos += Main.rand.NextVector2Circular(8f, 8f);

                Dust d = Dust.NewDustDirect(
                    pos,
                    0, 0,
                    DustID.Torch,
                    dir.X * 2f + Main.rand.NextFloat(-1f, 1f),
                    dir.Y * 2f + Main.rand.NextFloat(-1f, 1f),
                    150,
                    Color.OrangeRed,
                    1.4f
                );

                d.noGravity = true;
                d.velocity *= 1.3f;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            var texture = Terraria.GameContent.TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = new Vector2(20f, texture.Height / 2f);
            Vector2 drawPos = Projectile.Center - Main.screenPosition;
            float scale = 2f;

            Main.EntitySpriteDraw(
                texture,
                drawPos,
                null,
                lightColor,
                Projectile.rotation,
                origin,
                scale,
                SpriteEffects.None,
                0
            );

            return false;
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float collisionPoint = 0f;
            Vector2 start = Projectile.Center;
            Vector2 end = Projectile.Center + Projectile.rotation.ToRotationVector2() * 200f;

            return Collision.CheckAABBvLineCollision(
                targetHitbox.TopLeft(),
                targetHitbox.Size(),
                start,
                end,
                40f,
                ref collisionPoint
            );
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            target.AddBuff(BuffID.OnFire, 180); // 3 seconds
        }
    }
}