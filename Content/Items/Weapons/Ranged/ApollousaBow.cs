using Microsoft.Xna.Framework;
using NaturiumMod.Content.Items.Accessories.CraftingTrees.BaseGameCombos;
using NaturiumMod.Content.Items.Cards.Fusion;
using NaturiumMod.Content.Projectiles.Ranged;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Weapons.Ranged
{
    public class ApollousaBow : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Weapons/ApollousaBow";

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 64;
            Item.DamageType = DamageClass.Ranged;
            Item.damage = 160;
            Item.crit = 16;
            Item.knockBack = 4f;

            Item.useTime = 16;
            Item.useAnimation = 16;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;

            Item.shoot = ProjectileID.WoodenArrowFriendly;
            Item.shootSpeed = 14f;
            Item.useAmmo = AmmoID.Arrow;

            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.buyPrice(0, 8, 0, 0);
        }

        // Allow alt-function (right-click) — actual spawn handled by ModPlayer so it won't block normal firing
        public override bool AltFunctionUse(Player player) => true;

        public override bool CanUseItem(Player player)
        {
            bool titan = player.GetModPlayer<TitanociderPlayer>().hasTitanocider;

            if (player.altFunctionUse == 2)
            {
                // Right-click: charged shot settings (kept for consistency if used directly)
                Item.useTime = 64;
                Item.useAnimation = 64;
                Item.shootSpeed = 24f;
            }
            else if (titan)
            {
                // 3-shot burst (Eventide style)
                Item.useTime = 8;
                Item.useAnimation = 24;
                Item.shootSpeed = 16f;
            }
            else
            {
                // Normal single shot
                Item.useTime = 16;
                Item.useAnimation = 16;
                Item.shootSpeed = 16f;
            }

            return base.CanUseItem(player);
        }

        // Shoot handles normal and burst shots. We return false for alt to avoid double-spawning if someone right-clicks normally.
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source,
            Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            // Muzzle offset
            Vector2 muzzleOffset = Vector2.Normalize(velocity) * 40f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
                position += muzzleOffset;

            // If alt-function was used via normal Shoot path, skip (we handle right-click via ModPlayer)
            if (player.altFunctionUse == 2)
                return false;

            // 3-shot burst logic (Eventide-style)
            if (player.itemAnimation == player.itemAnimationMax && player.GetModPlayer<TitanociderPlayer>().hasTitanocider)
            {
                // Fire the real arrow only on the first burst shot
                Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            }
            else if (player.itemAnimation == player.itemAnimationMax && !player.GetModPlayer<TitanociderPlayer>().hasTitanocider)
            {
                // Normal single-shot behavior: fire the real arrow on first shot
                Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
            }

            // Goddess Arrow on every shot
            Projectile.NewProjectile(source, position, velocity * 1.1f, ModContent.ProjectileType<GoddessArrowProj>(),
                (int)(damage * 0.5f), knockback, player.whoAmI);

            // Titanocider synergy extra arrow on every shot
            if (player.GetModPlayer<TitanociderPlayer>().hasTitanocider)
            {
                Projectile.NewProjectile(source, position, velocity * 1.15f, ModContent.ProjectileType<GoddessArrowProj>(),
                    (int)(damage * 0.5f), knockback, player.whoAmI);
            }

            // Normal shot sound and light muzzle flash
            SoundEngine.PlaySound(SoundID.Item5, player.Center);

            for (int i = 0; i < 6; i++)
            {
                Dust d = Dust.NewDustDirect(position, 8, 8, DustID.HallowedTorch);
                d.noGravity = true;
                d.scale = 1.0f + Main.rand.NextFloat(0.0f, 0.4f);
                d.velocity = d.velocity.RotatedByRandom(MathHelper.ToRadians(20)) * Main.rand.NextFloat(0.4f, 1.0f);
            }

            return false;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.FairyQueenRangedItem)
                .AddIngredient(ItemID.HallowedBar, 33)
                .AddIngredient(ItemID.Ectoplasm, 22)
                .AddIngredient(ItemID.SoulofLight, 11)
                .AddIngredient(ModContent.ItemType<WindEssence>(), 50)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
    public class ApollousaPlayer : ModPlayer
    {
        private bool prevRight;
        private int rightClickCooldown; // ticks remaining until next allowed right-click shot

        // Change this if you want a fixed cooldown instead of using the item's useTime.
        // Set to 0 to use the held item's useTime automatically.
        private const int FixedCooldownTicks = 64;

        public override void PostUpdate()
        {
            // Only run for the local player (input is local)
            if (Player.whoAmI != Main.myPlayer)
            {
                prevRight = Main.mouseRight;
                return;
            }

            // Decrement cooldown if active
            if (rightClickCooldown > 0)
                rightClickCooldown--;

            // Check if player is holding the ApollousaBow
            int held = Player.HeldItem.type;
            int apollousaType = ModContent.ItemType<Content.Items.Weapons.Ranged.ApollousaBow>();

            if (held != apollousaType)
            {
                prevRight = Main.mouseRight;
                return;
            }

            // Detect a new right-click press (edge detection)
            bool rightNow = Main.mouseRight && Player.controlUseItem;
            bool pressed = rightNow && !prevRight;

            // Only allow firing if cooldown is zero
            if (pressed && rightClickCooldown == 0)
            {
                // Spawn charged shot
                Vector2 muzzlePos = Player.Center;
                Vector2 dir = Main.MouseWorld - muzzlePos;
                if (dir == Vector2.Zero) dir = new Vector2(Player.direction, 0);
                dir.Normalize();

                float speed = Player.HeldItem.shootSpeed * 1.5f;
                Vector2 projVel = dir * speed;

                Projectile.NewProjectile(
                    Player.GetSource_ItemUse(Player.HeldItem),
                    muzzlePos,
                    projVel,
                    ModContent.ProjectileType<Content.Projectiles.Ranged.GoddessJudgementProj>(),
                    Player.HeldItem.damage * 3,
                    Player.HeldItem.knockBack,
                    Player.whoAmI
                );

                // Heavy sound + effects
                SoundEngine.PlaySound(SoundID.Item14, Player.Center);

                for (int i = 0; i < 12; i++)
                {
                    var d = Dust.NewDustDirect(muzzlePos - new Vector2(6, 6), 12, 12, DustID.GoldFlame);
                    d.noGravity = true;
                    d.scale = 1.2f + Main.rand.NextFloat(0.2f, 0.6f);
                    d.velocity = d.velocity.RotatedByRandom(MathHelper.ToRadians(60)) * Main.rand.NextFloat(0.6f, 1.6f);
                }

                for (int i = 0; i < 4; i++)
                {
                    var d = Dust.NewDustDirect(muzzlePos, 8, 8, DustID.HallowedTorch);
                    d.noGravity = true;
                    d.scale = 1.6f;
                    d.velocity *= 0.5f;
                }

                // Small recoil
                Player.velocity += -dir * 0.6f;

                // Set cooldown: prefer FixedCooldownTicks if > 0, otherwise use the held item's useTime
                rightClickCooldown = (FixedCooldownTicks > 0) ? FixedCooldownTicks : Player.HeldItem.useTime;
            }

            prevRight = rightNow;
        }
    }
}
