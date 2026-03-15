using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.Cards;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Cards.LOB.Commons
{
    public class CelticGuardian : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/CelticGuardian";

        private int usesLeft = 9;
        private int swingIndex = 0;

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
            Item.damage = 20;
            Item.knockBack = 6f;

            Item.rare = ItemRarityID.Blue;
            Item.value = Item.buyPrice(silver: 25);

            Item.UseSound = SoundID.Item1;

            Item.shoot = ModContent.ProjectileType<CelticGuardianSlash>();
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

            // ⭐ increment swing index instead of XOR toggle
            swingIndex++;
            if (swingIndex >= 9)
            {
                swingIndex = 0;
                Item.stack--;     // ⭐ consume the card after 9 swings
                usesLeft = 9;     // reset for next card
            }

            // short cooldown
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
                    swingIndex % 3 // ⭐ 0=up→down, 1=down→up, 2=thrust
                );

                if (proj >= 0)
                    Main.projectile[proj].originalDamage = Item.damage;
            }

            return true;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source,
            Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return false; // prevent double spawn
        }
    }
    public class CelticGuardianSlash : ModProjectile
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/CelticGuardianSlash";

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

            Projectile.DamageType = ModContent.GetInstance<CardDamage>(); // ✔ FIXED

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;

            Projectile.spriteDirection = 1;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            var modPlayer = Main.player[Projectile.owner].GetModPlayer<WeaponBoostPlayer>();

            // If Warrior tag is active, apply the boost
            if (modPlayer.activeBoosts.TryGetValue("Warrior", out bool warriorActive) && warriorActive)
            {
                modifiers.SourceDamage *= 1.10f; // +10% damage
            }
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

            int swingType = (int)Projectile.ai[0] % 3;

            float swingRange = MathHelper.PiOver2;

            if (swingType == 0)
            {
                Projectile.rotation = baseRot + MathHelper.Lerp(swingRange, -swingRange, progress);
            }
            else if (swingType == 1)
            {
                Projectile.rotation = baseRot + MathHelper.Lerp(-swingRange, swingRange, progress);
            }
            else
            {
                Projectile.rotation = baseRot;
            }

            Projectile.Center = player.Center;

            player.direction = Main.MouseWorld.X >= player.Center.X ? 1 : -1;
            player.itemRotation = Projectile.rotation * player.direction;
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
            Vector2 end = Projectile.Center + Projectile.rotation.ToRotationVector2() * 115f;

            return Collision.CheckAABBvLineCollision(
                targetHitbox.TopLeft(),
                targetHitbox.Size(),
                start,
                end,
                20f,
                ref collisionPoint
            );
        }
    }
}
