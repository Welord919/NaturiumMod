using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NaturiumMod.Content.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.PreHardmode.Cards.UltraRares
{
    public class BEWD : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Cards/BEWD";

        public override void SetDefaults()
        {
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

        // Count how many BEWD cards the player has
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

        // Consume N BEWD cards
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

    // Base Summoning Sickness
    int duration = 600;

    // Cloak reduces sickness
    if (hasCloak && isDragon)
        duration = (int)(duration * 0.5);

    player.AddBuff(ModContent.BuffType<SummoningSickness>(), duration);

    // Count BEWD copies
    int bewdCount = CountBEWD(player);

    // ⭐ SPECIAL EFFECT: Kaiba’s Cloak + 3 BEWD
    if (hasCloak && bewdCount >= 3)
    {
        // Consume 3 cards
        ConsumeBEWD(player, 2);

        // Base direction toward cursor
Vector2 baseDir = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.UnitX);

// Rotation offsets
float[] angles =
{
    0f,
    MathHelper.ToRadians(10f),
    MathHelper.ToRadians(-10f)
};

foreach (float angle in angles)
{
    Vector2 dir = baseDir.RotatedBy(angle);

    int proj = Projectile.NewProjectile(
        Item.GetSource_FromThis(),
        player.Center,
        Vector2.Zero, // velocity ignored anyway
        ModContent.ProjectileType<DragonBurstCharge>(),
        200,
        4f,
        player.whoAmI,
        ai0: 0,
        ai1: angle
    );

    Main.projectile[proj].timeLeft *= 2;
}


        return true;
    }

    // ⭐ Normal single BEWD use
    if (player.whoAmI == Main.myPlayer)
    {
        Projectile.NewProjectile(
            Item.GetSource_FromThis(),
            player.Center,
            Vector2.Zero,
            ModContent.ProjectileType<DragonBurstCharge>(),
            200,
            4f,
            player.whoAmI
        );
    }

    return true;
}
    }
public class BurstStream : ModProjectile
    {
        public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Cards/BurstStream";
        public override void SetDefaults()
        {
            Projectile.width = 1000; // your PNG length
            Projectile.height = 40;  // your PNG height
            Projectile.friendly = true;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.timeLeft = 60; // lasts 1 second


            Projectile.usesLocalNPCImmunity = true;
            Projectile.localNPCHitCooldown = 10;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            // Beam starts at player
            Projectile.Center = player.Center;

            // Retrieve angle offset
            float angleOffset = Projectile.ai[0];

            // Base direction toward cursor
            Vector2 baseDir = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.UnitX);

            // Apply offset
            Vector2 finalDir = baseDir.RotatedBy(angleOffset);

            // Apply direction
            Projectile.velocity = finalDir;
            Projectile.rotation = finalDir.ToRotation();

            Lighting.AddLight(Projectile.Center, 0.3f, 0.4f, 1f);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;

            // ⭐ Origin at the START of the beam (left side)
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

            // 2000f = beam length
            return Collision.CheckAABBvLineCollision(
                targetHitbox.TopLeft(),
                targetHitbox.Size(),
                Projectile.Center,
                Projectile.Center + Projectile.velocity * 2000f,
                40f, // beam thickness
                ref collisionPoint
            );
        }
    }

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

            Projectile.timeLeft = 120; // enough time for charge + fire
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            // Stick to player
            Projectile.Center = player.Center;

            // FRAME 0: Lock direction + play roar
            if (Projectile.ai[0] == 0)
            {
                // ai[1] already contains the direction passed from BEWD


                SoundEngine.PlaySound(
                    SoundID.Roar with { Pitch = -0.4f, Volume = 1.2f },
                    Projectile.Center
                );
            }

            Projectile.ai[0]++; // frame counter

            // Charging dust
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

                // Base direction toward cursor
                Vector2 baseDir = (Main.MouseWorld - player.Center).SafeNormalize(Vector2.UnitX);

                // Apply the offset
                Vector2 finalDir = baseDir.RotatedBy(angleOffset);

                // Fire BurstStream and store ONLY the angle offset
                Projectile.NewProjectile(
                    Projectile.GetSource_FromThis(),
                    player.Center,
                    finalDir,
                    ModContent.ProjectileType<BurstStream>(),
                    Projectile.damage,
                    Projectile.knockBack,
                    player.whoAmI,
                    angleOffset, // ai[0] = offset
                    0            // ai[1] unused
                );
            }

        }
    }
}
