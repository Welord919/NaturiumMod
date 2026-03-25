using Microsoft.Xna.Framework;
using NaturiumMod.Content.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.PreHardmode.MillenniumItems
{

    public class MillenniumNecklace : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Millennium/MillenniumNecklace";
        public override void SetDefaults()
        {
            Item.width = 28;
            Item.height = 28;
            Item.accessory = true;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.buyPrice(gold: 5);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MillenniumNecklacePlayer>().MillenniumNecklaceEquipped = true;
            player.maxMinions += 1;
            player.GetKnockback(DamageClass.Summon) += 0.07f;
            player.GetAttackSpeed(DamageClass.Summon) += 0.07f;

            player.AddBuff(ModContent.BuffType<MillenniumVileEyeBuff>(), 2);

            if (player.ownedProjectileCounts[ModContent.ProjectileType<MillenniumVileEye>()] <= 0)
            {
                Projectile.NewProjectile(
                    player.GetSource_Accessory(Item),
                    player.Center,
                    Vector2.Zero,
                    ModContent.ProjectileType<MillenniumVileEye>(),
                    20,
                    2f,
                    player.whoAmI
                );
            }
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe = RecipeHelper.GetNewRecipe(recipe, [
                new(ModContent.ItemType<MillenniumPiece>(), 20),
            new(ItemID.Vilethorn, 1),
            new(ItemID.Amber, 10)
            ], TileID.Anvils);
            recipe.Register();
        }
    }
    public class MillenniumVileEyeBuff : ModBuff
    {
        public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Millennium/MillenniumRodBuff";
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            // If the minion projectile isn't alive, remove the buff
            if (player.ownedProjectileCounts[ModContent.ProjectileType<MillenniumVileEye>()] <= 0)
            {
                player.DelBuff(buffIndex);
                buffIndex--;
            }
        }

    }
    public class MillenniumNecklacePlayer : ModPlayer
    {
        public bool MillenniumNecklaceEquipped;

        public override void ResetEffects()
        {
            MillenniumNecklaceEquipped = false;
        }

    }
    public class MillenniumVileEye : ModProjectile
    {
        public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Millennium/MillenniumVileEye";
        private int shootCooldown = 0;

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 1;
            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.minionSlots = 1f;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;

        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            // Kill if player is gone or necklace is not equipped
            if (!player.active || player.dead ||
                !player.GetModPlayer<MillenniumNecklacePlayer>().MillenniumNecklaceEquipped)
            {
                Projectile.Kill();
                return;
            }
            // Keep the buff alive
            player.AddBuff(ModContent.BuffType<MillenniumVileEyeBuff>(), 2);

            // Position above the player
            Vector2 idlePosition = player.Center + new Vector2(0, -60);
            Projectile.Center = Vector2.Lerp(Projectile.Center, idlePosition, 0.1f);

            // Target enemies
            NPC target = FindTarget(player);

            if (target != null)
            {
                Projectile.rotation = Projectile.DirectionTo(target.Center).ToRotation();

                if (shootCooldown <= 0)
                {
                    ShootVilethorn(target);
                    shootCooldown = 60; // 1 second cooldown
                }
            }

            if (shootCooldown > 0)
                shootCooldown--;
        }

        private NPC FindTarget(Player player)
        {
            NPC chosen = null;
            float maxDistance = 500f;

            foreach (NPC npc in Main.npc)
            {
                if (npc.CanBeChasedBy() && Vector2.Distance(npc.Center, Projectile.Center) < maxDistance)
                {
                    maxDistance = Vector2.Distance(npc.Center, Projectile.Center);
                    chosen = npc;
                }
            }

            return chosen;
        }
        private void ShootVilethorn(NPC target)
        {
            Vector2 direction = Projectile.DirectionTo(target.Center);
            direction.Normalize();

            Projectile.NewProjectile(
                Projectile.GetSource_FromThis(),
                Projectile.Center,
                direction * 8f,
                ModContent.ProjectileType<MillenniumVilethorn>(),
                Projectile.damage,
                Projectile.knockBack,
                Projectile.owner
            );
        }
        public class MillenniumVilethorn : ModProjectile
        {
            public override string Texture => "NaturiumMod/Assets/Items/PreHardmode/Millennium/MillenniumVilethorn";
            public override void SetStaticDefaults()
            {
                Main.projFrames[Projectile.type] = 2; // 0 = tip, 1 = base
                ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
                ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;

            }
            public override void SetDefaults()
            {
                Projectile.width = 20;
                Projectile.height = 20;
                Projectile.friendly = true;
                Projectile.penetrate = -1;
                Projectile.tileCollide = false;
                Projectile.ignoreWater = true;
                Projectile.minion = true;
                Projectile.minionSlots = 0f;
                Projectile.DamageType = DamageClass.Summon;

                Projectile.timeLeft = 60;
                Projectile.extraUpdates = 1;
            }
            public override void AI()
            {
                Player player = Main.player[Projectile.owner];

                // If the player unequips the necklace, kill the minion
                if (!player.HasItem(ModContent.ItemType<MillenniumNecklace>()) &&
                    !player.GetModPlayer<MillenniumNecklacePlayer>().MillenniumNecklaceEquipped)
                {
                    Projectile.Kill();
                    return;
                }

                // First frame: lock rotation and set frame
                if (Projectile.localAI[0] == 0)
                {
                    Projectile.ai[1] = Projectile.velocity.ToRotation();
                    Projectile.rotation = Projectile.ai[1];
                    Projectile.frame = Projectile.ai[0] == 0 ? 0 : 1; // tip or base

                    // ⭐ Hit less often (local immunity)
                    Projectile.usesLocalNPCImmunity = true;
                    Projectile.localNPCHitCooldown = 12; // hits every 12 frames instead of every frame

                    Projectile.localAI[0] = 1;
                }
                // Lock direction forever
                Projectile.rotation = Projectile.ai[1];

                // ⭐ Slow down over time
                Projectile.velocity *= 0.98f; // stronger slowdown

                // ⭐ Prevent segments from drifting apart
                if (Projectile.velocity.Length() < 2f)
                    Projectile.velocity = Vector2.Zero; // freeze movement at the end

                // Spawn next segment (adjusted for doubled lifetime)
                if (Projectile.ai[0] < 6 && Projectile.timeLeft == 59)
                {
                    Projectile next = Projectile.NewProjectileDirect(
                        Projectile.GetSource_FromThis(),
                        Projectile.Center - Projectile.rotation.ToRotationVector2() * 20f,
                        Projectile.velocity, // same slowed velocity
                        Projectile.type,
                        Projectile.damage,
                        Projectile.knockBack,
                        Projectile.owner
                    );

                    next.ai[0] = Projectile.ai[0] + 1; // next segment index
                    next.ai[1] = Projectile.ai[1];     // same rotation
                }

                // Dust
                if (Main.rand.NextBool(3))
                {
                    int dust = Dust.NewDust(
                        Projectile.position,
                        Projectile.width,
                        Projectile.height,
                        DustID.CorruptGibs,
                        0f, 0f,
                        150,
                        default,
                        1.2f
                    );
                    Main.dust[dust].noGravity = true;
                }

                Lighting.AddLight(Projectile.Center, 0.3f, 0f, 0.4f);
            }
            public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
            {
                // Expand hitbox horizontally and vertically
                int expand = 12; // increase this to make it wider

                projHitbox.Inflate(expand, expand);

                return projHitbox.Intersects(targetHitbox);
            }
            public override bool? CanHitNPC(NPC target)
            {
                // Only the tip damages
                return Projectile.ai[0] == 0;
            }
        }
    }
}

