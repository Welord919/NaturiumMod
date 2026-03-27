using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.Cards.Fusion;
using NaturiumMod.Content.Items.Cards.LOB.Commons;
using NaturiumMod.Content.Items.PreHardmode.Materials;
using static Terraria.ModLoader.ModContent;

namespace NaturiumMod.Content.Items.PreHardmode.Weapons
{
    public class ReapersArm : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Weapons/ReapersArm";

        public override void SetDefaults()
        {
            Item.damage = 30;
            Item.DamageType = DamageClass.Magic;
            Item.mana = 3;
            Item.width = 26;
            Item.height = 26;
            Item.useTime = 60;
            Item.useAnimation = 20;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.channel = true;
            Item.knockBack = 4;
            Item.value = Item.sellPrice(silver: 60);
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item9;
            Item.shoot = ProjectileType<SpiritScytheProj>();
            Item.shootSpeed = 12f;
            Item.staff[Item.type] = true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe = RecipeHelper.GetNewRecipe(recipe, [
                new(ItemType<SkullServant>(), 10),
                new(ItemType<DarkEssence>(), 5),
                new(ItemID.ZombieArm, 1),
                new(ItemID.Bone, 20)
            ], TileID.Anvils);
            recipe.Register();
        }
    }

    public class SpiritScytheProj : ModProjectile
    {
        public override string Texture => "NaturiumMod/Assets/Items/General/Projectiles/SpiritScytheProj";

        private const int MouseOffsetY = -24;
        private int manaTimer = 0;

        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 45;
            Projectile.friendly = true;
            Projectile.light = 0.8f;
            Projectile.DamageType = DamageClass.Magic;

            DrawOriginOffsetX = -25;
            DrawOriginOffsetY = -22;

            Projectile.penetrate = -1;
            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;

            Projectile.tileCollide = false; // goes through walls
            Projectile.timeLeft = 2;        // channel weapon behavior
        }

        public override void OnSpawn(IEntitySource source)
        {
            SoundEngine.PlaySound(SoundID.Item9, Projectile.position);
        }

        public override Color? GetAlpha(Color lightColor) => new Color(255, 255, 255, 0);

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            // Keep alive while channeling OR until released
            Projectile.timeLeft = 2;

            // -----------------------------
            // ⭐ MANA DRAIN (every 20 ticks)
            // -----------------------------
            manaTimer++;
            if (manaTimer >= 20)
            {
                manaTimer = 0;

                int manaCost = 2;

                if (player.statMana >= manaCost)
                {
                    player.statMana -= manaCost;
                    player.manaRegenDelay = 60; // prevent regen while channeling
                }
                else
                {
                    Projectile.Kill();
                    return;
                }
            }

            // -----------------------------
            // 2. CHANNELING MODE
            // -----------------------------
            if (player.channel && Projectile.ai[0] == 0f)
            {
                Vector2 bottomLeft = Projectile.position + new Vector2(0, Projectile.height + MouseOffsetY);

                Projectile.rotation += 0.25f;

                float maxDistance = 18f;
                Vector2 toCursor = Main.MouseWorld - bottomLeft;
                float dist = toCursor.Length();

                if (dist > maxDistance)
                {
                    dist = maxDistance / dist;
                    toCursor *= dist;
                }

                Projectile.velocity = toCursor;

                // Store direction for release
                if (toCursor != Vector2.Zero)
                {
                    Projectile.localAI[0] = toCursor.X;
                    Projectile.localAI[1] = toCursor.Y;
                }

                return;
            }

            // -----------------------------
            // 3. RELEASE MODE → FLY OFF
            // -----------------------------
            if (Projectile.ai[0] == 0f)
            {
                Projectile.ai[0] = 1f;
                Projectile.netUpdate = true;

                Vector2 stored = new Vector2(Projectile.localAI[0], Projectile.localAI[1]);
                if (stored == Vector2.Zero)
                    stored = Vector2.UnitX;

                stored.Normalize();
                Projectile.velocity = stored * 12f;

                Projectile.tileCollide = false; // stays ghost-like
            }

            // -----------------------------
            // 4. RELEASED → KEEP SPINNING
            // -----------------------------
            Projectile.rotation += 0.35f;
        }

        // -----------------------------
        // ROTATING LINE HITBOX
        // -----------------------------
        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float bladeLength = 50f;
            float bladeThickness = 12f;

            Vector2 pivot = Projectile.position + new Vector2(0, Projectile.height + MouseOffsetY);

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
}