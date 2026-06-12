using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace NaturiumMod.Content.Items.Accessories
{
    [AutoloadEquip(EquipType.Wings)]
    public class SteamedCloak : ModItem
    {
        public override string Texture => "NaturiumMod/Assets/Items/Accessories/Black-WingedWings";

        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 34;
            Item.accessory = true;
            Item.rare = ItemRarityID.Yellow;
            Item.value = Item.buyPrice(gold: 10);

            ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(150, 7.5f, 2f);
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // Keep the cloak's passive effects
            player.longInvince = true;                 // Cross Necklace effect
            player.shimmerImmune = true;               // Shimmer cloak effect
            player.moveSpeed += 0.10f;
            player.wingRunAccelerationMult += 0.10f;

            // Mark the player as having the Steamed Cloak equipped so the ModPlayer can run its logic
            player.GetModPlayer<SteamedCloakPlayer>().steamedCloakEquipped = true;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ModContent.ItemType<BlackWingedWings>())
                .AddIngredient(ItemID.BeeCloak)
                .AddIngredient(ItemID.CrossNecklace)
                .AddIngredient(ModContent.ItemType<LeodrakesMane>())
                .AddIngredient(ItemID.ShimmerCloak)
                .AddTile(TileID.TinkerersWorkbench)
                .Register();
        }
    }
    public class SteamedCloakPlayer : ModPlayer
    {
        // Equipped flag set by the accessory
        public bool steamedCloakEquipped;

        // Cooldown in ticks (360 = 6 seconds)
        private int cloakCooldown = 0;

        // Track the last stage we fired for cooldown bypass logic (-1 = none)
        private int lastStageFired = -1;

        // Stage thresholds (fractions of max life)
        // stage 0: >= 75% -> 6 slashes, multiplier 1.0
        // stage 1: < 75%  -> 8 slashes, multiplier 1.1
        // stage 2: < 50%  -> 10 slashes, multiplier 1.2
        // stage 3: < 25%  -> 12 slashes, multiplier 1.3
        private readonly int[] slashCounts = new int[] { 6, 8, 10, 12 };
        private readonly float[] damageMultipliers = new float[] { 1.0f, 1.1f, 1.2f, 1.3f };

        public override void ResetEffects()
        {
            steamedCloakEquipped = false;
        }

        public override void PostUpdate()
        {
            // Decrement cooldown each tick
            if (cloakCooldown > 0)
                cloakCooldown--;

            // If player healed back up above the last fired stage, allow re-firing at that stage later
            int currentStage = GetCurrentStage();
            if (lastStageFired > currentStage)
            {
                // Player moved to a higher-health (lower stage) region; reset lastStageFired so bursts can trigger again
                lastStageFired = -1;
            }
        }

        public override void OnHurt(Player.HurtInfo hurtInfo)
        {
            if (!steamedCloakEquipped)
                return;

            // Determine which stage applies right now (highest applicable)
            int stage = GetCurrentStage();

            // If nothing applies (shouldn't happen), return
            if (stage < 0 || stage >= slashCounts.Length)
                return;

            // If cooldown active and we haven't reached a new lower threshold, do nothing
            if (cloakCooldown > 0 && stage <= lastStageFired)
                return;

            // Fire the burst for the current (highest) stage
            FireBurst(stage);

            // Set cooldown and record stage fired
            cloakCooldown = 360; // 6 seconds
            lastStageFired = stage;
        }

        private int GetCurrentStage()
        {
            if (Player.statLife <= Player.statLifeMax2 * 0.25f)
                return 3;
            if (Player.statLife <= Player.statLifeMax2 * 0.50f)
                return 2;
            if (Player.statLife <= Player.statLifeMax2 * 0.75f)
                return 1;
            return 0;
        }

        private void FireBurst(int stage)
        {
            int amount = slashCounts[stage];
            float damageMultiplier = damageMultipliers[stage];

            // Play a sound
            SoundEngine.PlaySound(SoundID.Roar with { Volume = 0.8f, Pitch = 0.1f }, Player.Center);

            // Base damage for the slashes (match your BlackGaleSlash base if desired)
            int baseDamage = 20;
            int finalDamage = (int)Math.Round(baseDamage * damageMultiplier);

            float speed = 8f;
            // Spread the projectiles evenly in a circle
            for (int i = 0; i < amount; i++)
            {
                float angle = MathHelper.TwoPi * i / amount;
                Vector2 velocity = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * speed;

                Projectile.NewProjectile(
                    Player.GetSource_OnHurt(Player),
                    Player.Center,
                    velocity,
                    ModContent.ProjectileType<BlackGaleSlash>(),
                    finalDamage,
                    2f,
                    Player.whoAmI
                );
            }
        }
    }
}
