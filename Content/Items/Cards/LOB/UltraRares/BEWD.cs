using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.Cards;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Cards.LOB.UltraRares
{
    public class BEWD : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/BEWD";

        public override void SetDefaults()
        {
            Item.damage = 200;
            Item.DamageType = ModContent.GetInstance<CardDamage>();

            Item.width = 32;
            Item.height = 32;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 20;
            Item.useTime = 20;
            Item.UseSound = SoundID.Item4;
            Item.consumable = true;
            Item.maxStack = 999;
            Item.rare = ItemRarityID.LightRed;
            Item.value = Item.buyPrice(gold: 3);
            Item.shoot = ProjectileID.None;
            Item.autoReuse = true;
            Item.noUseGraphic = true;
            Item.noMelee = true;
        }

        private int CountBEWD(Player player)
        {
            int count = 0;
            foreach (var item in player.inventory)
            {
                if (item.type == ModContent.ItemType<BEWD>())
                    count += item.stack;
            }
            return count;
        }

        private void ConsumeBEWD(Player player, int amount)
        {
            for (int i = 0; i < player.inventory.Length && amount > 0; i++)
            {
                if (player.inventory[i].type == ModContent.ItemType<BEWD>())
                {
                    int take = Math.Min(player.inventory[i].stack, amount);
                    player.inventory[i].stack -= take;
                    amount -= take;

                    if (player.inventory[i].stack <= 0)
                        player.inventory[i].TurnToAir();
                }
            }
        }

        public override bool? UseItem(Player player)
        {
            bool hasCloak = player.GetModPlayer<KaibaPlayer>().KaibasCloakEquipped;
            bool isDragon = WeaponTag.ItemTags.TryGetValue(Type, out var tags) && tags.Contains("Dragon");

            int duration = 600;
            if (hasCloak && isDragon)
                duration = (int)(duration * 0.5);

            player.AddBuff(ModContent.BuffType<SummoningSickness>(), duration);

            int bewdCount = CountBEWD(player);

            // Triple-shot effect
            if (hasCloak && bewdCount >= 3)
            {
                ConsumeBEWD(player, 2);

                Vector2 baseDir = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.UnitX);

                float[] angles =
                {
                    0f,
                    MathHelper.ToRadians(10f),
                    MathHelper.ToRadians(-10f)
                };

                foreach (float angle in angles)
                {
                    Projectile.NewProjectile(
                        Item.GetSource_FromThis(),
                        player.Center,
                        Vector2.Zero,
                        ModContent.ProjectileType<DragonBurstCharge>(),
                        Item.damage,        // ✔ CardDamage scaling works
                        Item.knockBack,
                        player.whoAmI,
                        ai0: 0,
                        ai1: angle
                    );
                }

                return true;
            }

            // Single shot
            if (player.whoAmI == Main.myPlayer)
            {
                Projectile.NewProjectile(
                    Item.GetSource_FromThis(),
                    player.Center,
                    Vector2.Zero,
                    ModContent.ProjectileType<DragonBurstCharge>(),
                    Item.damage,        // ✔ CardDamage scaling works
                    Item.knockBack,
                    player.whoAmI
                );
            }

            return true;
        }
    }

    // ============================================================
    // BURST STREAM (BEAM)
    // ============================================================

    public class BurstStream : ModProjectile
    {
        public override string Texture => "NaturiumMod/Assets/Items/Cards/LOB/BurstStream";

        public override void SetDefaults()
        {
            Projectile.width = 1000;
            Projectile.height = 40;
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 60;

            Projectile.DamageType = ModContent.GetInstance<CardDamage>(); // ✔ CardDamage

            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            var modPlayer = Main.player[Projectile.owner].GetModPlayer<WeaponBoostPlayer>();

            // If Warrior tag is active, apply the boost
            if (modPlayer.activeBoosts.TryGetValue("KaibaBoost", out bool warriorActive) && warriorActive)
            {
                modifiers.SourceDamage *= 1.10f; // +10% damage
            }
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            Projectile.Center = player.Center;

            float angleOffset = Projectile.ai[0];

            Vector2 baseDir = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.UnitX);
            Vector2 finalDir = baseDir.RotatedBy(angleOffset);

            Projectile.velocity = finalDir;
            Projectile.rotation = finalDir.ToRotation();

            Lighting.AddLight(Projectile.Center, 0.3f, 0.4f, 1f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
            Vector2 origin = new Vector2(0, tex.Height / 2f);

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

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            float collisionPoint = 0f;

            return Collision.CheckAABBvLineCollision(
                targetHitbox.TopLeft(),
                targetHitbox.Size(),
                Projectile.Center,
                Projectile.Center + Projectile.velocity * 2000f,
                40f,
                ref collisionPoint
            );
        }
    }

    // ============================================================
    // DRAGON BURST CHARGE
    // ============================================================

    public class DragonBurstCharge : ModProjectile
    {
        public override string Texture => "NaturiumMod/Assets/Items/General/Projectiles/Blank";

        public override void SetDefaults()
        {
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.friendly = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.DamageType = ModContent.GetInstance<CardDamage>(); // ✔ CardDamage
            Projectile.timeLeft = 120;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            Projectile.Center = player.Center;

            if (Projectile.ai[0] == 0)
            {
                SoundEngine.PlaySound(
                    SoundID.Roar with { Pitch = -0.4f, Volume = 1.2f },
                    Projectile.Center
                );
            }

            Projectile.ai[0]++;

            for (int i = 0; i < 4; i++)
            {
                Dust d = Dust.NewDustDirect(
                    Projectile.position,
                    Projectile.width,
                    Projectile.height,
                    DustID.BlueCrystalShard,
                    Main.rand.NextFloat(-3, 3),
                    Main.rand.NextFloat(-3, 3),
                    150,
                    Color.White,
                    1.8f
                );
                d.noGravity = true;
            }

            Lighting.AddLight(Projectile.Center, 0.3f, 0.4f, 1f);

            if (Projectile.ai[0] == 60 && Main.myPlayer == Projectile.owner)
            {
                float angleOffset = Projectile.ai[1];

                Vector2 baseDir = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.UnitX);
                Vector2 finalDir = baseDir.RotatedBy(angleOffset);

                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    player.Center,
                    finalDir,
                    ModContent.ProjectileType<BurstStream>(),
                    Projectile.damage,      // ✔ inherits BEWD damage WITH scaling
                    Projectile.knockBack,
                    player.whoAmI,
                    angleOffset
                );
            }
        }
    }
}