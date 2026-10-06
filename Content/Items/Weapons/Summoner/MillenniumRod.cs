using Microsoft.Xna.Framework;
using NaturiumMod.Content.Helpers;
using NaturiumMod.Content.Items.Materials;
using NaturiumMod.Content.Projectiles.Melee;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Weapons.Summoner
{
    public class MillenniumRod : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Millennium/MillenniumRod";

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;

            Item.useTime = 25;
            Item.useAnimation = 25;

            // Shoot style allows HoldoutOffset() to work.
            Item.useStyle = ItemUseStyleID.Shoot;

            Item.rare = ItemRarityID.Yellow;
            Item.noMelee = true;
            Item.DamageType = DamageClass.Summon;

            Item.mana = 10;
            Item.value = Item.buyPrice(gold: 5);

            Item.damage = 10;
            Item.knockBack = 2f;

            Item.buffType = ModContent.BuffType<MillenniumRodBuff>();
            Item.shoot = ModContent.ProjectileType<MillenniumEye>();

            // We handle the projectile/summon ourselves in Shoot().
            Item.shootSpeed = 0f;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();

            recipe = RecipeHelper.GetNewRecipe(recipe, [
                new(ModContent.ItemType<MillenniumPiece>(), 15),
            new(ItemID.CrimsonRod, 1),
            new(ItemID.Amber, 10),
        ], TileID.Anvils);

            recipe.Register();
        }

        public override bool AltFunctionUse(Player player) => true;

        public override Vector2? HoldoutOffset()
        {
            return new Vector2(-6f, 0f);
        }

        public override bool CanUseItem(Player player)
        {
            // Don't perform the right-click effect here.
            // We still want Terraria to actually use the item so
            // the rod is displayed during the use animation.
            return true;
        }

        public override bool Shoot(
            Player player,
            EntitySource_ItemUse_WithAmmo source,
            Vector2 position,
            Vector2 velocity,
            int type,
            int damage,
            float knockback)
        {
            // =========================================================
            // RIGHT CLICK — Confusion Pulse
            // =========================================================
            if (player.altFunctionUse == 2)
            {
                if (player.HasBuff(BuffID.Slow))
                {
                    if (Main.myPlayer == player.whoAmI)
                    {
                        Main.NewText(
                            "The Millennium Rod needs time to recharge...",
                            Color.Gray
                        );
                    }

                    return false;
                }

                SoundEngine.PlaySound(
                    SoundID.Item27 with
                    {
                        Volume = 0.8f,
                        Pitch = -0.2f
                    },
                    player.Center
                );

                ConfuseNearbyEnemies(player);

                // 5 second recharge.
                player.AddBuff(BuffID.Slow, 300);

                // We don't want to actually fire MillenniumEye
                // on right-click.
                return false;
            }

            // =========================================================
            // LEFT CLICK — Summon Millennium Eye
            // =========================================================

            SoundEngine.PlaySound(
                SoundID.Item44 with
                {
                    Volume = 0.8f,
                    Pitch = -0.1f
                },
                player.Center
            );

            // Apply the summon buff.
            player.AddBuff(Item.buffType, 2);

            // Spawn the minion.
            Projectile.NewProjectile(
                source,
                player.Center,
                Vector2.Zero,
                type,
                damage,
                knockback,
                player.whoAmI
            );

            // We manually created the projectile.
            return false;
        }

        private void ConfuseNearbyEnemies(Player player)
        {
            float radius = 300f;

            foreach (NPC npc in Main.ActiveNPCs)
            {
                if (!npc.CanBeChasedBy())
                    continue;

                if (Vector2.Distance(npc.Center, player.Center) <= radius)
                {
                    npc.AddBuff(BuffID.Confused, 180);
                }
            }
        }
    }

    public class MillenniumRodBuff : ModBuff
    {
        public override string Texture => "NaturiumMod/Assets/Items/Millennium/MillenniumRodBuff";

        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<MillenniumEye>()] > 0)
                player.buffTime[buffIndex] = 18000;
            else
                player.DelBuff(buffIndex);
        }
    }

    public class MillenniumEye : ModProjectile
    {
        public override string Texture => "NaturiumMod/Assets/Items/Millennium/MillenniumEye";

        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.MinionSacrificable[Type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 20;
            Projectile.height = 20;

            Projectile.friendly = false;
            Projectile.hostile = false;

            Projectile.minion = true;
            Projectile.minionSlots = 1f;

            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 18000;

            Projectile.DamageType = DamageClass.Summon;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            if (!player.HasBuff(ModContent.BuffType<MillenniumRodBuff>()))
            {
                Projectile.Kill();
                return;
            }

            // Keep buff alive and projectile alive
            player.AddBuff(ModContent.BuffType<MillenniumRodBuff>(), 2);
            Projectile.timeLeft = 2;

            // --- Orbit configuration (stable, equally spaced, smooth) ---
            const float baseOrbitRadius = 60f;   // distance from player
            const float rotationSpeed = 0.008f;  // radians per tick (tweak to taste)
            const float smoothing = 0.35f;       // 0-1 lerp factor for smooth movement

            // Collect owned MillenniumEye projectiles and sort by whoAmI for stable ordering
            List<int> owned = new List<int>();
            for (int i = 0; i < Main.maxProjectiles; i++)
            {
                Projectile p = Main.projectile[i];
                if (p.active && p.owner == Projectile.owner && p.type == Projectile.type)
                    owned.Add(p.whoAmI);
            }

            owned.Sort();

            int count = owned.Count;
            if (count <= 0) count = 1; // fallback

            int myIndex = owned.IndexOf(Projectile.whoAmI);
            if (myIndex < 0) myIndex = 0;

            float step = MathHelper.TwoPi / count;
            float globalRotation = Main.GameUpdateCount * rotationSpeed;

            float angle = globalRotation + myIndex * step;

            Vector2 targetOffset = baseOrbitRadius * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
            Vector2 targetPos = player.Center + targetOffset;

            // Smoothly move to target position to avoid snapping/flicker
            Projectile.Center = Vector2.Lerp(Projectile.Center, targetPos, smoothing);

            // Optional: face outward toward movement/target
            Projectile.rotation = (targetPos - Projectile.Center).ToRotation();

            // Targeting and firing (kept behavior, cleaned)
            NPC target = FindTarget(player, 500f);
            if (target != null)
                FireAtTarget(player, target);
        }

        private NPC FindTarget(Player player, float range)
        {
            NPC closest = null;
            float dist = range;

            foreach (NPC npc in Main.ActiveNPCs)
            {
                if (!npc.CanBeChasedBy()) continue;

                float d = Vector2.Distance(player.Center, npc.Center);
                if (d < dist)
                {
                    dist = d;
                    closest = npc;
                }
            }

            return closest;
        }

        private void FireAtTarget(Player player, NPC target)
        {
            if (Projectile.localAI[0] > 0)
            {
                Projectile.localAI[0]--;
                return;
            }

            Projectile.localAI[0] = 30;

            Vector2 direction = (target.Center - Projectile.Center).SafeNormalize(Vector2.UnitX) * 10f;

            SoundEngine.PlaySound(SoundID.Item20 with { Volume = 0.7f, Pitch = -0.2f }, Projectile.Center);

            Projectile.NewProjectile(
                Projectile.GetSource_FromThis(),
                Projectile.Center,
                direction,
                ModContent.ProjectileType<ApophisProj>(),
                Projectile.originalDamage,
                0f,
                player.whoAmI
            );
        }
    }
}
